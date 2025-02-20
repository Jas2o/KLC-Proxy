# KLC-Proxy 
Proxy is a replacement URI handler written in C# to relaunch/redirect Kaseya VSA 9.5 agent endpoint launch requests that were intended for Live Connect. It was functional up to VSA 9.5.20 however will not receive any further VSA testing/development.

It can be used to:
- Relaunch Live Connect launch requests after they were closed or crashed.
- Redirect requests to KLC-Finch, which is an alternative frontend to Live Connect.
- Watch an endpoint's online/offline status and fire a launch request or display a notification when it comes online.
- Override launch requests to act as if the user had hovered over an agent orb in VSA (which clicking would normally only launch remote control), waited for the popup to appear, and pressed "Live Connect" button.
- Load in saved agents into relaunch/watch list without needing to click them from VSA (however this requires at least one click from VSA to load authorisation token before this works).

The main reason this exists is because years ago Live Connect was really unstable, I was constantly needing to relaunch it while assisting clients and doing it through VSA web interface was slow. This also opened up the possibly of being able to switch between Live Connect and KLC-Finch depending on my needs as Finch was really fast and stable but not always compatible after Kaseya VSA/agent updates.

## First usage
- Run KLC-Proxy, open Settings.
- Set "From VSA" to "Use KLC-Proxy".
  - This changes registry at: HKEY_CURRENT_USER\Software\Classes\liveconnect\shell\open\command
- Either leave "On Remote Control" on "Original" (Kaseya Live Connect) or change it to "Alternative" (KLC-Finch).
- The other settings can be left on "Same as Remote Control".
- Close the Settings window and close KLC-Proxy.

## Continued usage each day
- From VSA, make at least one connection to an agent that would normally trigger Live Connect/remote control.
- KLC-Proxy should get launched and run that last launch request.
- You can now do anything that requires an authorisation token to be loaded, replay and redirect launch requests.
- Apps menu then Auth Token can save the token to Windows Credential Manager temporarilly.
  - This is needed before launching Ex/Explorer, or Finch directly for testing using agent GUIDs instead of from VSA.

## Required other repos to build
- LibKaseya
- LibKaseyaAuth

## Required packages to build
- CredentialManagement.Standard
- Newtonsoft.Json
- nucs.JsonSettings
- Ookii.Dialogs.Wpf
- RestSharp
