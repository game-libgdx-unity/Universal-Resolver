# EditorPrefs Editor
[![No Maintenance Intended](http://unmaintained.tech/badge.svg)](http://unmaintained.tech/)

Hello! This is Christian, the author of EditorPrefs Editor. I hope you're liking the extension!

## What is EPEditor?
**EPEditor** (a.k.a. **EditorPrefs Editor**, but that's a mouthful so let's go with the short version) is a small extension for **Unity** in which you can add a new or existing EditorPref, and keep track of it.

While working on my editor extensions, I often need to check EditorPrefs, be it for internal stuff, make sure that preferences are set or initialized correctly, etc. The lack of an interface to keep track of them has been very frustrating at times.

Enter EPEditor: with it you can **add and track EditorPrefs in a visual way**.
You can create new Prefs (which will be added to the Unity ones automatically), you can retrieve existing ones, and you can also edit
and remove them.

## How does it work?
You can use EPEditor in two ways:

The first is by using the interface - you can either add a new EditorPref or retrieve the value of one whose key and type you know. When they're part of the "database" you can edit and remove them individually.

The second way to use it is in conjunction with code - the extension includes an EditorPrefs wrapper called NewEditorPrefs, which works the same as the Unity one (it relies on their own functions, as a matter of fact), but is also integrated with the interface of the extension, so that you can add an EditorPref from code and have it automatically added to the EPEditor interface.

It's a fairly small and simple extension, but seeing is believing and I love gifs, so I suggest going to its [webpage](http://immortalhydra.com/stuff/editorprefs-editor/ "EPEditor page on immortalhydra.com") to see everything the extension has to offer :)

## How do I install it?
S'easy!

You can find all versions in the [_Release/_](https://github.com/HHErebus/GDTB_EPEditor/tree/master/Release) folder. They are ordered by year and release number.

1. Download the most recent release.
2. Open the Unity project you want to import EPEditor.
3. Double click on the file you downloaded.
4. The Import Package window will pop up, click "Import", and voilá!

The files are packaged with the same Unity version used to upload to the Asset Store, so Unity may ask you to "upgrade" or "update" it. You can do so safely, I go through the process myself before uploading a newer version.

## Where can I find more information?
You can definitely write me if you want to! You can reach me [@hherebus](https://www.twitter.com/hherebus "Twitter") on Twitter, or by writing me a quick email at support@immortalhydra.com

ASCII titles realized with the [Text ASCII Art Generator](http://patorjk.com/software/taag/) ("ANSI Shadow" and "Calvin S" styles).
