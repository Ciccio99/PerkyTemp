# PerkyTemp

## BLE Temperature Monitoring iOS App

## Team

- Jake Hartz
- Alberto Scicali
- Mitul Sheth

# Usage

Run on the iOS platform with iOS 10 or greater installed.

## How to deploy
- May need to use free provisioning to deploy the app to an iOS 10+ device
    - Look here for how to do it (https://developer.xamarin.com/guides/ios/getting_started/installation/device_provisioning/free-provisioning/)
- In the `info.plist` under the `PerkyTemp.iOS` folder, set your bundle identifier to the one you created in the previous Free Provisioning step.
    - ![alt text](/Pics/infoplistImage.png "info.plist image")
- Plug in your iOS device via USB cable, and in visual studio select the debug device to be your iOS device.
    - ![alt text](/Pics/debugSelection1.png "debug selection")
    - ![alt text](/Pics/debugDeviceSelection.png "Sebug device selection")
- Once your device is selected, make sure it is unlocked and press the Play button in visual studio to deploy the app to your device.


# Goal 

To monitor the temperature on ThermApparel vests and provide users with a
simple and clean mobile app that allows them to turn temperature tracking on
and off, as well as see the previous temperature tracking sessions.


# Features

- Current Vest Temperature
- Start/Stop Vest Tracking
- Vest Duration Prediction
- User Tracking Sessions

## Design Features

- Simple, Clean
- Med-Big Font Size
- Easy to Read Font Type
- Color Scheme similar to ThermApparel's website


# MVVM Structure

### View

- Main Window includes: Start/Stop Recording, current temperature, cooling time left
- Settings window: Final temperature timeout warning/notification, interval
  notifications (warning how much time is left at each interval)
- Sessions/History window: Past temperature recording sessions (date, cooling
  duration, start temp, end temp)

### ViewModels

- Main window
- Settings window
- Sessions/History window

### Models

- Past Session Model
- Settings Model (persistent storage)
- Notifications Model
- BLE (Connection, search for BLE devices, translate bytes to temp)
- Current session model (Current changes in temp, get current temperature, get
  time remaining, etc.)
