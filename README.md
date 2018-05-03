[![](http://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE)
# Kisaragi
Kisaragi is TimeSignal Application for .NET

![KisaragiInfo](KisaragiInfo.png)
![form](form.png)

## Kisaragi is 何？
作業に没頭する人向けの、時間お知らせサポートアプリですっっ('ω')  

## 主な機能
- 1時間毎の通知 (24h)
- アラーム機能 (任意の時間にて設定できます。設定単位：分)
- Twitter 連携機能 (ライブラリ：自作, 通知メッセージを投稿)
- 音声ファイルを使用することにより、通知をより分かりやすくできます。

## History
2018-03-13 時点：  
- タスクトレイ機能に対応中 => 完了
- UML 図のコミット完了(今後、適宜修正が入ります。) => 実施中  

2018-03-15 時点：  
- Twitter へ投稿するために、投稿ライブラリを作成しました。まだコミットしてません。
- version 1.0 リリースに向けて、絶賛リファクタリング中。  

2018-03-21 時点： 
- OAuth1.0a 及び 2.0 に対応するべく、全体的な アクセスAPI を構築中。
　認証は Pass したが、 GET, POST の投稿で Failed している状況。

2018-04-23 時点： 
- Ver1.0 リリースに向けて最終調整中。  
  Usage 等に関しても記載していきます。  
  もうしばらくお待ちください。
  
2018-05-03 時点：
- 全体的な機能見直しを実施  
  - アラーム機能を追加  
  - 音声ファイルがなくても、アプリ動作が可能に  
  - Twitter 連携時の認証キーを GUI 上から入力可能に  
  
- 使用方法を記載中。
 
## License for Kisaragi
MIT License

## 使用した OS, IDE など
- OS
  - Windows 10 Home

- IDE
  - Visual Studio 2017 Community (C# 7.1, .NET Framework 4.7.1)
  - Visual Studio Code 1.12
  - SmartGit
  
### Kisaragiで利用しているライブラリとそのライセンス
- Newtonsoft.Json : MIT License
- JsonFx : MIT License
