# MiFare Card to Active Directory

Intention is to build a windows form app that can be installed for any user on premise and allow them to use a locked down service account to write MiFare numbers into AD.

## Service Account
Instruction from internet, not tested yet.
- Open Active Directory Users and Computers (ADUC).
- Create a new user (e.g., svc_UpdateExtensionAttr).
- Assign a strong password and set "Password never expires".
- Do not add this account to any privileged groups (e.g., Domain Admins).
- Right-click the domain root or OU where user accounts reside (depending on your scope).
- Click "Delegate Control…".
- Add your service account.
- Choose "Create a custom task to delegate".
- Select "Only the following objects in the folder", check "User objects".
- On the Permissions screen:
  - Check "Property-specific".
  - Locate and check only the attribute you want (e.g., extensionAttribute1).
  - Note: The extensionAttribute fields are usually displayed as "ms-Exch-Extension-Attribute-1" etc. in the GUI.
  - Finish the wizard.
