# blackscreen
exe実行で接続されている画面をすべてブラックアウトするアプリケーション(windows)

## 概要
blackscreen.exeを実行すると，接続されている画面がすべてブラックアウトします．解除するには，マウスで画面をクリックする必要があります．
デフォルトは1回です．

## 発行
csファイル内のclickを変更することでクリック数を調整できます．
以下のコマンドでアプリケーションを発行し，exeファイルを作成できます．
bin/Release/net6.0/win-x64/publish/にexeファイルが作成されます．

```
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true --self-contained true
```
