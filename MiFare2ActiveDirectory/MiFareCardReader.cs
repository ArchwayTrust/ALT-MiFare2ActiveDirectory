using PCSC;
using PCSC.Exceptions;
using PCSC.Monitoring;
using PCSC.Iso7816;
using System;

namespace MiFare2ActiveDirectory
{
    public class MiFareCardReader
    {
        private readonly SCardContext _context;
        private readonly SCardMonitor _monitor;

        public event EventHandler<string>? CardRead;

        public List<String>? CardReaderNames;
        public string? CardReaderName;

        public MiFareCardReader()
        {
            _context = new SCardContext();
            _context.Establish(SCardScope.System);

            _monitor = new SCardMonitor(new ContextFactory(), SCardScope.System);
            _monitor.CardInserted += OnCardInserted;
        }

        public void StartMonitoring()
        {
            _monitor.Start(this.CardReaderName);
        }

        public void StopMonitoring()
        {
            _monitor.Cancel();
        }

        public void GetAvailableReaders()
        {
            try
            {
                String[] readers = _context.GetReaders();
                this.CardReaderNames = [.. readers];
            }
            catch
            {
            }
        }

        private void OnCardInserted(object? sender, CardStatusEventArgs e)
        {
            try
            {
                using var reader = new SCardReader(_context);
                var rc = reader.Connect(e.ReaderName, SCardShareMode.Shared, SCardProtocol.Any);
                if (rc != SCardError.Success)
                {
                    throw new PCSCException(rc, rc.ToString());
                }

                var apdu = new CommandApdu(IsoCase.Case2Short, reader.ActiveProtocol)
                {
                    CLA = 0xFF,
                    INS = 0xCA,
                    P1 = 0x00,
                    P2 = 0x00,
                    Le = 0x00
                };

                var sendPci = SCardPCI.GetPci(reader.ActiveProtocol);
                var receivePci = new SCardPCI();
                var receiveBuffer = new byte[256];

                var command = apdu.ToArray();
                var receiveBufferLength = receiveBuffer.Length;

                rc = reader.Transmit(sendPci, command, command.Length, receivePci, receiveBuffer, ref receiveBufferLength);
                if (rc != SCardError.Success)
                {
                    throw new PCSCException(rc, rc.ToString());
                }

                // Exclude the last two bytes (SW1 and SW2) from the card number
                var cardNumberLength = receiveBufferLength - 2;
                var cardNumber = BitConverter.ToString(receiveBuffer, 0, cardNumberLength).Replace("-", string.Empty);
                CardRead?.Invoke(this, cardNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading card: {ex.Message}");
            }
        }
    }
}
