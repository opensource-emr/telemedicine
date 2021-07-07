import React, { useState, useEffect, useRef } from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import global from './Components/Global';
import { HubConnectionBuilder } from "@microsoft/signalr";
//import Loader from './Components/Loader';
import JitsiMeet, { JitsiMeetView } from 'react-native-jitsi-meet';
const LiveVideoScreen = ({ navigation }) => {
  // const [loading, setLoading] = useState(true);
  const [showMeet, setShowMeet] = React.useState(false);
  useEffect(() => {
    connect = new HubConnectionBuilder()
      .withUrl(global.url + "NotificationHub?token=" + global.token)
      .withAutomaticReconnect()
      .build();
    connect.serverTimeoutInMilliseconds = 1000 * 60 * 120;
    connect.keepAliveIntervalInMilliseconds = 1000 * 15;
    connect.start();

    connect.on('CallPatient', (data) => {
      var obj = JSON.parse(data);
      console.log(obj)
      startMeeting();

    });
    connect.on('GetAllProviders', (data) => {
      const jsonData = JSON.parse(data);
      console.log(jsonData);
    });
    connect.on('CallEnds', (data) => {
      var obj = JSON.parse(data);
      console.log(obj)
    });

    connect.on('CompletePatient', (data) => {
      var obj = JSON.parse(data)
      console.log(obj)
      global.patient = obj;
      connect.stop();
      completeVisit();
    });
    fetch(global.url + "Practice/GetUpdatedProvider?username="
      + global.obj.url + "&practiceName=" + global.obj.practice, {
      method: "GET",
      headers: {
        'Authorization': 'Bearer ' + global.token
      }
    })
      .then(
        res => res.json()
      )
      .then(json => {
        global.provider = json.User
      })
      .catch((error) =>
        console.error(error)
      )
  }, []);
  const startMeeting = () => {
    setShowMeet(true)
    const url = 'https://meet.jit.si/' + global.provider.roomName;
    const userInfo = { displayName: global.obj.name };
    JitsiMeet.call(url, userInfo);
  }

  const completeVisit = () => {
    JitsiMeet.endCall();
    navigation.replace('ReportScreen');
  }

  return (
    <View style={styles.container}>

      {showMeet && (
        <JitsiMeetView
          style={{
            flex: 10,
            height: '100%',
            width: '100%',
          }}
        />
      )}
      <TouchableOpacity>
        {
          !showMeet ? (<Text style={styles.button}>Please wait {global.provider} will join meet soon</Text>
          ) : null}

      </TouchableOpacity>
    </View>
  );
};
const styles = StyleSheet.create({
  container: {
    flex: 10,
    alignItems: 'center',
    justifyContent: 'center',
  },
  button: {
    borderRadius: 10,
    width: 275,
    height: 50,
    padding: 10,
    borderWidth: 2,
    borderColor: "gray",
    justifyContent: "center",
    alignItems: "center",
  }
});
export default LiveVideoScreen;