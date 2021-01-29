; Copyright 2017 Brian Dagan
; MIT License:
; Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
; The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
; THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

; If multiple instances are started, this will shut down existing instances and start just this one
#SingleInstance, force

; Define the application name, icon and text strings
ApplicationName = KRC and KLC Force-Paste Helper
ApplicationIconDLL = shell32.dll
ApplicationIconIndex = 261
PasteNotEnabled = A Kaseya window needs to be active to paste.
PasteSuccessful = Successfully force-pasted clipboard's contents into foreground window
PasteFailed = Unable to force-paste clipboard's contents into foreground window (clipboard does not contain text)
PasteTooLong = Clipboard contents is too long to paste!
AboutText = A utility to force-paste clipboard's contents into the foreground window, toggle Scroll Lock and then use the Middle Mouse button to auto-type.

; Update the tray icon and tray tip to match the branding above
Menu, Tray, Icon, %ApplicationIconDLL%, %ApplicationIconIndex%
Menu, Tray, Tip, %ApplicationName%

; Remove the standard tray icon options and replace with an Exit and About dialog
Menu, Tray, NoStandard
Menu, Tray, Add, &About, About
Menu, Tray, Add, E&xit, Exit

; Set the keystroke delay to 0 (smallest possible delay, normally defaults to 10ms between keystrokes)
SetKeyDelay, 10

; Fire a force-paste when Ctrl+Shift+V is pressed
MButton::
{
	If WinActive("ahk_exe KaseyaLiveConnect.exe") or WinActive("ahk_exe KLCAlt.exe") {
		; Check the clipboard's format to ensure it contains plain text
		If DllCall("IsClipboardFormatAvailable", "Uint", 1) {
			; The clipboard contains plain text... send the keystrokes to the foreground window
			temp = %Clipboard%
			
			if(StrLen(temp) <= 40) {		
				Loop, Parse, temp
				{
					SendRaw, %A_LoopField%
					Sleep 2
					Send, {Shift up}
					Sleep 2
				}
				TrayTip, %ApplicationName%, %PasteSuccessful%, 5, 49
			} Else {
				TrayTip, %ApplicationName%, %PasteTooLong%, 5, 51
			}
		} Else {
			; The clipboard does not contain plain text, notify the user via. TrayTip
			TrayTip, %ApplicationName%, %PasteFailed%, 5, 51
		}
	} Else {
		; TrayTip, %ApplicationName%, %PasteNotEnabled%, 5, 50
		Send {MButton}
	}
	Return
}

About:
{
    TrayTip, %ApplicationName%, %AboutText%, 5, 49
    Return
}

Exit:
{
    ExitApp
    Return
}