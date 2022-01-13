# triple

[![WebGL Demo](https://img.shields.io/badge/demo-WebGL-orange.svg?style=flat&logo=google-chrome&logoColor=white&cacheSeconds=2592000)](https://danyow.cn/webgl)

```yml
# 正式用户
UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
UNITY_SERIAL: ${{ secrets.UNITY_SERIAL }}
# 免费用户
UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
```

## UNITY_EMAIL

用于登录和构建_Unity for Linux_ 时候, 你的 _Unity_ 的电子邮件地址

## UNITY_PASSWORD

用于登录和构建_Unity for Linux_ 时候, 你的 _Unity_ 的密码

## UNITY_SERIAL

如果你是订阅了 _Unity Plus_ 或者 _Unity Pro_ 之后, 那么直接可以从[Unity 订阅页面](https://id.unity.com/en/subscriptions)获取密钥.

## UNITY_LICENSE

1. 创建一个文件, 加入以下代码, 并放置到 `.github/workflows/getManualLicenseFile.yml`

   ```yml
   name: 获取激活文件
   on:
     workflow_dispatch: {}
   jobs:
     activation:
       name: 请求手动激活文件 🔑
       runs-on: ubuntu-latest
       steps:
         # 请求手动激活文件
         - name: 请求手动激活文件
           id: getManualLicenseFile
           uses: game-ci/unity-request-activation-file@v2
         # Upload artifact (Unity_v20XX.X.XXXX.alf)
         - name: 导出
           uses: actions/upload-artifact@v2
           with:
             name: ${{ steps.getManualLicenseFile.outputs.filePath }}
             path: ${{ steps.getManualLicenseFile.outputs.filePath }}
   ```

2. 推送该文件并手动运行该`Action`等待片刻得到一个`Unity_v20XX.X.XXXX.alf`并下载保存解压
3. 访问[手动激活 Unity 许可证](https://license.unity3d.com/manual)
4. 上传刚刚得到的`.alf`文件
    1. 可能会有 _serial has reached the maximum number of activations._ 这个问题的出现, 目前没有好的解决方案.
    2. 我的解决方案是创建了一个新的账号, 这个账号不在个人电脑上操作.
5. 得到一个 `Unity_v20XX.x.ulf` 文件, 里面的内容便是.

> 注意：更改 Unity 版本时，您可能需要重复相同的过程。

## GITHUB_TOKEN

~~1. 这个不需要填写, `Github Action` 文档里面的意思好像会自动填写.~~

所有以 `GITHUB` 开头的都不允许填写.
详见对应[文档](https://docs.github.com/cn/actions/security-guides/automatic-token-authentication#permissions-for-the-github_token)

## ANDROID_KEYSTORE_BASE64

将整个 `.keystore`文件 _base64_ 处理, 用于构建安卓包.

### ~~_windows_ 环境下使用~~

[certutil][certutil]

该方法并不适用

1. ~~生成~~
   `certutil -encode release.keystore release.base64` 只需要复制里面的内容即可
2. ~~验证~~
   `certutil -decode release.base64 release_decode.keystore`

### _linux_ 环境

安装并直接调用 _base64_ 库即可

1. 生成
   `base64 release.keystore > release.base64`
2. 验证
   `cat release.base64 | base64 --decode > release_comp.keystore`

## ANDROID_KEYSTORE_PASS

keystore密钥中, 密钥库的密码

## ANDROID_KEYALIAS_NAME

keystore密钥中, 密钥库中选中的密钥名称

## ANDROID_KEYALIAS_PASS

keystore密钥中, 密钥库中选中的密钥密码

## GH_PERSONAL_ACCESS_TOKEN

`Github` 的 **Personal access token**, 用来上传包到 `Github` 的发布页面

## GH_PERSONAL_ACCESS_TOKEN_FULL

`Github` 的 **Personal access token**, 用来上传内容到 `Github` 


## UNITY_EMAIL_2

用于登录和构建_Unity for Windows_ 时候, 你的 _Unity_ 的电子邮件地址

## UNITY_PASSWORD_2

用于登录和构建_Unity for Windows_ 时候, 你的 _Unity_ 的密码

## UNITY_AUTHENTICATOR_KEY_2

用于登录和构建_Unity for Windows_ 时候, 激活账号使用权限的密钥

1. 登录 _Unity_ 帐户
2. 转到帐户设置
3. 通过身份验证器应用程序激活双因素身份验证
4. 在带有 **QR** 码的页面上，单击"无法扫描条形码？"并保存密钥（删除其中的空格）
5. 完成激活

## GOOGLE_PLAY_KEY_FILE

获取 API 访问权限 利用 Google Play Developer Publishing API 实现应用配置自动化，并将应用版本整合到您现有的工具和流程中。

1. 打开 _Google Play 管理中心(Google Play Console)_
2. 单击 _帐户详情_ ，然后下滑找到并记下其中列出的 _开发者帐号 ID_
3. 单击 _设置_ → _API 权限_
4. 单击 _选择要关联的项目_
5. 单击 _我同意_ → _创建新项目_ → _关联项目_
6. 在 **服务账号** 这一栏目里面有 单击 _创建新的服务账号_.
7. 在弹出的对话框中的 _Google 云端平台(Google Cloud Platform)_ 链接，该链接将打开一个新标签页：
    1. 在 **搜索框** 左边有一个 下来框, 如果显示 **选择项目**, 需要下来选择到 _Google Play Console Developer_
    2. 然后 _创建服务账号_
        1. 输入 _服务账号名称_ → 输入 _服务账号说明_
        2. 点击 _创建并继续_
        3. 单击 _请选择一个角色_ → 筛选框内输入 `Service Account User` 并选中筛选结果.
        4. 直接点击继续
    3. 得到一个账号后, 在`操作`那里有垂直三点图标, 点开选中 _管理密钥_ → _添加密钥_ → 选择 `JSON` 后直接创建.
    4. 这时候浏览器将会提示将文件保存在计算机上，并记住文件保存到的位置.
8. 返回 _Google Play 管理中心_ 标签页，然后点击 _完成_ 以关闭对话框
9. 然后底部就会出现一个新的服务账户 单击 _授予访问权_.
10. 建议直接选择 _管理员(所有权限)_ → _邀请用户_ → _发送邀请_
11. 完成.

```yaml
ios:
  APPLE_CONNECT_EMAIL: ${{ secrets.APPLE_CONNECT_EMAIL }}
  APPLE_DEVELOPER_EMAIL: ${{ secrets.APPLE_DEVELOPER_EMAIL }}
  APPLE_TEAM_ID: ${{ secrets.APPLE_TEAM_ID }}
  APPLE_TEAM_NAME: ${{ secrets.APPLE_TEAM_NAME }}
  MATCH_URL: ${{ secrets.MATCH_URL }}
  MATCH_PERSONAL_ACCESS_TOKEN: ${{ secrets.GH_PERSONAL_ACCESS_TOKEN }}
  MATCH_PASSWORD: ${{ secrets.MATCH_PASSWORD }}
  APPSTORE_KEY_ID: ${{ secrets.APPSTORE_KEY_ID }}
  APPSTORE_ISSUER_ID: ${{ secrets.APPSTORE_ISSUER_ID }}
  APPSTORE_P8: ${{ secrets.APPSTORE_P8 }}
  USYM_UPLOAD_AUTH_TOKEN: ${{ secrets.USYM_UPLOAD_AUTH_TOKEN }}

mac:
  APPLE_CONNECT_EMAIL: ${{ secrets.APPLE_CONNECT_EMAIL }}
  APPLE_DEVELOPER_EMAIL: ${{ secrets.APPLE_DEVELOPER_EMAIL }}
  APPLE_TEAM_ID: ${{ secrets.APPLE_TEAM_ID }}
  APPLE_TEAM_NAME: ${{ secrets.APPLE_TEAM_NAME }}
  APPLE_DISTRIBUTION_CERTIFICATE: ${{ secrets.APPLE_DISTRIBUTION_CERTIFICATE }}
  APPLE_DISTRIBUTION_PASSWORD: ${{ secrets.APPLE_DISTRIBUTION_PASSWORD }}
  MAC_INSTALLER_CERTIFICATE: ${{ secrets.MAC_INSTALLER_CERTIFICATE }}
  MAC_INSTALLER_PASSWORD: ${{ secrets.MAC_INSTALLER_PASSWORD }}
  APPSTORE_KEY_ID: ${{ secrets.APPSTORE_KEY_ID }}
  APPSTORE_ISSUER_ID: ${{ secrets.APPSTORE_ISSUER_ID }}
  APPSTORE_P8: ${{ secrets. APPSTORE_P8 }}

  ${{ secrets.MICROSOFT_STORE_PFX_FILE }}

MICROSOFT_TENANT_ID: ${{ secrets.MICROSOFT_TENANT_ID }}
MICROSOFT_CLIENT_ID: ${{ secrets.MICROSOFT_CLIENT_ID }}
MICROSOFT_KEY: ${{ secrets.MICROSOFT_KEY }}

username: ${{ secrets.STEAM_USERNAME }}
password: ${{ secrets.STEAM_PASSWORD }}
configVdf: ${{ secrets.STEAM_CONFIG_VDF }}
ssfnFileName: ${{ secrets.STEAM_SSFN_FILE_NAME }}
ssfnFileContents: ${{ secrets.STEAM_SSFN_FILE_CONTENTS }}

DISCORD_WEBHOOK: ${{ secrets.DISCORD_WEBHOOK }}
consumer-key: ${{ secrets.TWITTER_CONSUMER_API_KEY }}
consumer-secret: ${{ secrets.TWITTER_CONSUMER_API_SECRET }}
access-token: ${{ secrets.TWITTER_ACCESS_TOKEN }}
access-token-secret: ${{ secrets.TWITTER_ACCESS_TOKEN_SECRET }}
```

[certutil]: https://docs.microsoft.com/zh-cn/windows-server/administration/windows-commands/certutil