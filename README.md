# triple

```yml
# 正式用户
UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
UNITY_SERIAL: ${{ secrets.UNITY_SERIAL }}
# 免费用户
UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
```

## UNITY_EMAIL

添加您用于登录 _Unity_ 的电子邮件地址

## UNITY_PASSWORD

添加用于登录 _Unity_ 的密码

## UNITY_SERIAL

订阅了 _Unity Plus_ 或者 _Unity Pro_ 之后, 可以从[Unity 订阅页面](https://id.unity.com/en/subscriptions)获取密钥.

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

1. 这个不需要填写, `Github Action` 文档里面的意思好像会自动填写.

## ANDROID_KEYSTORE_BASE64

其实也就是整个文件 _base64_ 处理

### _windows_ 环境下使用[certutil](https://docs.microsoft.com/zh-cn/windows-server/administration/windows-commands/certutil)

1. 生成
   `certutil -encode release.keystore release.base64`
2. 验证
   `certutil -decode release.base64 release_decode.keystore`

### _linux_ 环境

安装并直接调用 _base64_ 库即可

`base64 release.keystore`

## ANDROID_KEYSTORE_PASS

密钥库的密码

## ANDROID_KEYALIAS_NAME

密钥的名称

## ANDROID_KEYALIAS_PASS

密钥的密码
