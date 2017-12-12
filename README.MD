# PerkyTemp

## BLE Temperature Monitoring iOS App

## Team

- Jake Hartz
- Alberto Scicali
- Mitul Sheth

# Usage
- Run on iOS platform with iOS 10 or greater installed

## Goal 

To monitor the temperature on ThermApparel vests and provide users with a simple and clean mobile app 
that allows them to turn temperature tracking on and off, as well as see the previous temperature tracking sessions.


## To-Do

- Find multi platform BLE component/plugin 
- Determine Requirements
- Quick Wireframe
- Sketch out MVVM Layout for app

# Features

- Current Vest Temperature
- Start/Stop Vest Tracking
- Vest Duration Prediction
- User Tracking Sessions (Record session data if Tracking for 5+ mins)
- Linked to weather/temp somehow?

# Design Features

- Simple, Clean
- Med-Big Font Size
- Easy to Read Font Type
- Color Scheme similar to ThermApparel's website


# MVVM Structure

### View
  
- Main Window includes: Start/Stop Recording, current temperature, cooling time left, Maybe outside weather temp
- Settings window: Final temperature timeout warning/notification, 
    interval notifications (warning how much time is left at each interval), turn on/off native notifications, 
    pairing mechanism with BLE temp sensor
- Sessions/History window: Had past temperature recording sessions (Date, cooling duration, start temp, end temp)

### ViewModels

- Main window
- Settings window
- Sessions/History window

### Models

- Past Session Model (Store BLE sensor GUID or MAC with session)
- Settings Models (Persistent storage)
- Notifications Model
- BLE (Connection, search for BLE devices, translate bytes to temp)
- Current session model (Current changes in temp, get current temperature, get time remaining, etc)

