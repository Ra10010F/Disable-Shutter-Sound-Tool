# Disable Shutter Sound Tool
国内板Samsung Galaxyシリーズのスマホのシャッター音を無効化するツールです。 adbを使って処理しているのでadbの環境が必須になります。

不具合等があればXの@Ra16fatttまでDMください。

使わせて頂いたツール [15-Seconds-Online-ADB-Installer-and-Updater-jp](https://github.com/reindex-ot/15-Seconds-Online-ADB-Installer-and-Updater-jp)

※このツールを使用して不具合が生じたとしても一切の責任を負いません。

## 詳細
adbが使用可能か確認し、使用可能であれば「adb shell settings get system csc_pref_camera_forced_shuttersound_key」を使って0か1かを判別する。

結果に応じて有効化、無効化のボタンを押せるようにする。
