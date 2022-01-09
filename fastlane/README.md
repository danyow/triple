fastlane documentation
----

# 安装

确保您安装了最新版本的 Xcode 命令行工具：

```sh
xcode-select --install
```

有关 _fastlane_ 安装说明，请参阅[Installing _fastlane_](https://docs.fastlane.tools/#installing-fastlane)

# 可用操作

## 安卓

### android internal

```sh
[bundle exec] fastlane android internal
```

将新的 Android 版本上传到 Google Play 商店(内部测试)

### android alpha

```sh
[bundle exec] fastlane android alpha
```

将新的 Android 版本上传到 Google Play 商店(Alpha测试)

### android beta

```sh
[bundle exec] fastlane android beta
```

将新的 Android 版本上传到 Google Play 商店(开放测试)

### android production

```sh
[bundle exec] fastlane android production
```

将新的 Android 版本上传到 Google Play 商店(生产)

----


## iOS

### ios release

```sh
[bundle exec] fastlane ios release
```

向 App Store 交付新的 Release 版本

### ios beta

```sh
[bundle exec] fastlane ios beta
```

为 Apple TestFlight 提供新的 Beta 版本

### ios build

```sh
[bundle exec] fastlane ios build
```

创建.ipa

----


## Mac

### mac fixversion

```sh
[bundle exec] fastlane mac fixversion
```

破解以便 Apple 不会因为版本控制错误而拒绝 mac 构建

### mac macupload

```sh
[bundle exec] fastlane mac macupload
```

将新的 Mac 版本上传到 Mac App Store

----

此 README.md 是自动生成的，每次运行 [_fastlane_](https://fastlane.tools) 时都会重新生成。

关于 _fastlane_ 的更多信息可以在 [fastlane.tools](https://fastlane.tools) 上找到。

_fastlane_ 的文档可以在 [docs.fastlane.tools](https://docs.fastlane.tools) 上找到。
