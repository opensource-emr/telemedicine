import React, { useState, useEffect } from 'react';
import { View, TextInput, Text, SafeAreaView, ImageBackground,
Button, StyleSheet, TouchableOpacity, Image, ScrollView} from 'react-native';
import { color } from 'react-native-reanimated';
import global from './Components/Global';
import RNHTMLtoPDF from 'react-native-html-to-pdf';
import RNPrint from 'react-native-print';

const ReportScreen = ({ navigation }) => {
  const [userEmail, setUserEmail] = useState('');
  const [errortext, setErrortext] = useState(''); 
  const [succsesstext, setSuccsessext] = useState('');
  const [followUpNo, setFollowUpNo] = React.useState(false);
  const [advice, setAdvice] = React.useState(false);
  const [Slash, setSlash] = useState('');
  
  const handlePatientValid = (userEmail) => {
    if (!userEmail) {
      setErrortext('Email is required');
      setTimeout(() => {
        setErrortext();
      }, 3000);
      return;
    }
    else {
      setErrortext('')
    }
  }
  useEffect(() => {
    if(global.patient.followUpNumber != ""){
      setFollowUpNo(true);
    } 
    if(global.patient.medication != ""){
      setAdvice(true);
    } 
    
    if(global.patient.followUpNumber != ""){
      setSlash("/");
     }
    global.providerAdvice = "";
    for (var i = 0; i < global.patient.advice.length; i++) {
      adv = global.patient.advice[i].advice;
      value = global.patient.advice[i].isChecked == true ? 'Yes' : 'No';
      global.providerAdvice += "<tr><td style ='font-size:25;padding:10px;'>" 
        + adv + ":&nbsp;" + value + "</td></tr>";
    }
  }, []);

  const handleEmailSendPress = () => {
    // setErrortext('');
      const reg = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
      if (reg.test(userEmail) === true){
        setSuccsessext('Report has been sent to your email')
        setTimeout(() => {
          setSuccsessext();
          }, 3000);
      }
      else{
        setErrortext('Invalid email');
        setTimeout(() => {
          setErrortext();
          }, 3000);
          return;
       }
    const key = "73l3M3D";
    global.patient.email = userEmail;
    global.patient.key = key;
    fetch(global.url + 'Messenger/EmailPatientReport?key=' + key, {
      method: 'POST',
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
        'Authorization': 'Bearer ' + global.token
      },

      body: JSON.stringify(
        global.patient,
      )
    })
      .then(
        res => res.json()
      )
      .then(json => {
        console.log(json)
      }

      )
      .catch((error) =>
        console.error(error)
      )
  };
   async function createPDF() {
    let options = {
      html:  
        '<h1 style="text-align:center;font-size:50;font-weight:bold;">' + global.practice + '</h3>' +
        '<h2 style="text-align:center;font-size:30;font-weight:bold;">Thank You For Consulting ' + global.obj.providerNameAttending+ '</h2>' +
        '<h2 style="text-align:center;font-size:30;font-weight:bold;">Visit Summary</h2><br>' +
        '<table style="width:100%">' +
        '<tr>' +
        '<td style="font-size:25;padding:10px;">What we did today? ' + global.providerAdvice  + '</td>' +
        '</tr>' +
        '<tr>' +
        '<td style="font-size:25;padding:10px;">Advice: ' + global.patient.medication + '</td>' +
        '</tr>' +
        '<tr>' +
        '<td style="font-size:25;padding:10px;">Follow Up in: ' +global.patient.followUpNumber+ Slash + global.patient.followUpMeasure+ 
        '</td>' +
        '</tr>' +
        '</table>',
      fileName: 'Report',
      directory: 'Documents',
    };

    let file = await RNHTMLtoPDF.convert(options)
    // console.log(file.filePath);
    alert(file.filePath);
  }

  // const printHTML = async () => {
  //   await RNPrint.print({
  //     html:
  //       '<h1 style="text-align:center;font-size:50;font-weight:bold;">' + global.practice + '</h3>' +
  //       '<h2 style="text-align:center;font-size:30;font-weight:bold;">Thank You For Consulting ' + global.provider + '</h2>' +
  //       '<h2 style="text-align:center;font-size:30;font-weight:bold;">Visit Summary</h2><br>' +
  //       '<table style="width:100%">' +
  //       '<tr>' +
  //       '<td style="font-size:25;padding:10px;">Follow Up in: ' + global.providerAdvice  + '</td>' +
  //       '</tr>' +
  //       '<tr>' +
  //       '<td style="font-size:25;padding:10px;">Advice: ' + global.patient.medication + '</td>' +
  //       '</tr>' +
  //       '<tr>' +
  //       '<td style="font-size:25;padding:10px;">Follow Up in: ' +global.patient.followUpNumber+ slash + global.patient.followUpMeasure+ 
  //       '</td>' +
  //       '</tr>' +
  //       '</table>'
  //   });
  // };

  return (
    <View style={styles.mainBody}>
      <ScrollView
        keyboardShouldPersistTaps="handled"
        contentContainerStyle={{
          flex: 1,
          justifyContent: 'center',
          alignContent: 'center',
      }}>
          <ImageBackground
          source={require('../Image/patient-bg.png')}
          style={{
            flex: 1,
            width: 420,
            height: 700,
            resizeMode: 'contain',
            padding: 20
          }}>
          <View style={styles.registerTextStyle}>
            <Text style={{ fontSize: 25, fontWeight: 'bold' }}>{global.practice}</Text>
          </View>
          <View style={{ backgroundColor: 'white', padding: 10 }}>
            <View style={{
              padding: 5, alignItems: 'center', justifyContent: 'center', fontSize: 30,
              textAlign: 'center',
              marginBottom: 20
            }}>
              <Text style={{ fontSize: 20, fontWeight: 'bold' }}>Thank You For Consulting {global.obj.providerNameAttending}</Text>
            </View>
            <View style={{
              borderColor: 'gray', padding: 5,
              borderWidth: 1.0
            }}>
              <View style={{
                padding: 5, alignItems: 'center', justifyContent: 'center', fontSize: 30,
                textAlign: 'center',
                marginBottom: 20
              }}>
                <Text style={{ fontSize: 20, fontWeight: 'bold' }}>Visit Summary</Text>
              </View >
              <View style={styles.contentTextStyle1}>
                <Text style={{ fontSize: 16, fontWeight: 'bold' }}>What we did today?  </Text>
                {
                  global.patient.advice.map((item, idx) => {
                    return <View key={idx} style={{ flexDirection: 'column' }}>
                      <Text style={{ fontSize: 16, color: 'gray' }}>{item.advice} :  {item.isChecked ? 'Yes' : 'No'}</Text>
                    </View>
                  })
                }
              </View>
              <View style={styles.contentTextStyle}>
                <Text style={{ fontSize: 16, fontWeight: 'bold' }}>{advice ?(<Text> Advice :</Text> ): null}</Text>
                <Text style={{ fontSize: 16, color: 'gray' }}>{advice ?(<Text> {global.patient.medication}</Text>): null}</Text>
              </View>
              <View style={styles.contentTextStyle}>
                <Text style={{ fontSize: 16, fontWeight: 'bold' }}>{followUpNo ?(<Text> Follow Up in :</Text>): null}</Text>
                <Text style={{ fontSize: 16, color: 'gray' }}> {
                followUpNo ?(<Text> {global.patient.followUpNumber}{Slash}{global.patient.followUpMeasure}</Text> ): null}</Text>
              </View>
            </View>
            <View style={{
              padding: 10, alignItems: 'center', justifyContent: 'center', fontSize: 30,
              textAlign: 'center', fontWeight: 'bold',
              marginBottom: 20
            }}>
            <Text style={{ fontSize: 16, fontWeight: 'bold' }}>Send your report to your email!.</Text>
            </View>
            <View style={{alignItems: 'flex-start', justifyContent: 'space-between', fontSize: 30, flexDirection: 'row',}}>
              <View style={styles.SectionStyle}>
                <TextInput
                  style={styles.emailStyle}
                  onChangeText={(UserEmail) =>
                    setUserEmail(UserEmail)
                  }
                  placeholder="Enter email address " //dummy@abc.com
                  placeholderTextColor="#8b9cb5"
                  autoCapitalize="none"
                  keyboardType="email-address"
                  returnKeyType="next"
                  underlineColorAndroid="#f000"
                  blurOnSubmit={false}
                  onEndEditing={(e) => handlePatientValid(e.nativeEvent.text)}
                />
              </View>
              <TouchableOpacity
                style={styles.buttonStyle}
                activeOpacity={0.5}
                onPress={handleEmailSendPress}>
                <Text style={styles.buttonTextStyle}>Send</Text>
              </TouchableOpacity>
              <Text style={{fontWeight: 'bold', color: 'gray', fontSize: 20,marginTop:5 }}>OR</Text>
              <View>
               {/* <TouchableOpacity onPress={printHTML}> */}
               <TouchableOpacity onPress={createPDF}>
                  <Image
                  source={require('../Image/PrintSVG.png')}
                    style={styles.image}/>
                </TouchableOpacity>
              <Text style={{ fontWeight: 'bold' }}>Print</Text>
              </View>
            </View>
            {errortext != '' ? (
                <Text style={styles.errorTextStyle}>
                  {errortext}
                </Text>
              ) : null}
              {succsesstext != '' ? (
                <Text style={styles.successTextStyle}>
                  {succsesstext}
                </Text>
              ) : null}
          </View>
        </ImageBackground>
    </ScrollView>
  </View>
  );
};

export default ReportScreen;

const styles = StyleSheet.create({
  mainBody: {
    flex: 1,
    justifyContent: 'center',
    alignContent: 'center',
  },
  buttonTextStyle: {
    color: '#FFFFFF',
    paddingVertical: 10,
    fontSize: 16,
  },
  registerTextStyle: {
    color: '#FFFFFF',
    textAlign: 'center',
    alignSelf: 'center',
    padding: 10,
  },

  contentTextStyle: {
    padding: 5, alignItems: 'flex-start', justifyContent: 'flex-start',
    textAlign: 'center',
    marginBottom: 20, flexDirection: 'row'
  },
  contentTextStyle1: {
    padding: 5, alignItems: 'flex-start', justifyContent: 'flex-start',
    textAlign: 'center',
    marginBottom: 20
  },
  SectionStyle: {
    flexDirection: 'row',
    height: 40,
    width: 60,
    color: 'black',
  },
  emailStyle: {
    flex: 1,
    color: 'black',
    minWidth: 145,
    borderWidth: 1,
    borderColor: 'gray',
  },
  buttonStyle: {
    backgroundColor: '#3F51B5',
    borderWidth: 0,
    color: 'white',
    borderColor: '#7DE24E',
    height: 40,
    alignItems: 'center',
    width: 70,
    marginLeft: 80,
  },
  buttonStyle1: {
    backgroundColor: '#3F51B5',
    borderWidth: 0,
    color: 'white',
    borderColor: '#7DE24E',
    height: 40,
    alignItems: 'center',
    width: 60
  },
  errorTextStyle: {
    color: 'red',
    textAlign: 'left',
    fontSize: 14,
  },
  successTextStyle: {
    color: 'blue',
    textAlign: 'left',
    fontSize: 14,
  },
  image: {
    height: 30,
    width: 30,
  },
});