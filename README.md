# Read Me

## ページの追加方法

- content/に.mdファイルを追加
- pages/配下に.mdファイルと同名の.vueファイルを追加

## デプロイ方法

- yarn generate
  - 静的ファイルの生成
- yarn deploy
  - gh-pagesブランチを利用したgithub pagesへのデプロイ

## 参考

- [gh-pages](https://github.com/lucpotage/nuxt-github-pages)
- ※dist配下に`.nojekeyll`を追加
