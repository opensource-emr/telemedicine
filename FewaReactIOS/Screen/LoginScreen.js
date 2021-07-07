import React, { useState, createRef } from 'react';
import {
  StyleSheet,
  TextInput,
  View,
  Text,
  ScrollView,
  Image,
  TouchableOpacity,
  KeyboardAvoidingView,
} from 'react-native';
import global from './Components/Global';

const LoginScreen = ({ navigation }) => {
  const [userEmail, setUserEmail] = useState('');
  const [providerName, setProviderName] = useState('');
  const [practiceName, setPracticeName] = useState('');
  const [errortext, setErrortext] = useState('');
  const [errortext1, setErrortext1] = useState('');
  const [errortext2, setErrortext2] = useState('');

  const passwordInputRef = createRef();

  const handlePatientValid = (userEmail) => {
    if (!userEmail) {
      setErrortext('Patient name required');
      setTimeout(() => {
        setErrortext();
      }, 3000);
      return;
    }
    else {
      setErrortext('')
    }
  }
  const handleProviderValid = (providerName) => {
    if (!providerName) {
      setErrortext1('Provider name required');
      setTimeout(() => {
        setErrortext1();
      }, 3000);
      return;
    }
    else {
      setErrortext1('')
    }
  }
  const handlePracticeValid = (practiceName) => {
    if (!practiceName) {
      setErrortext2('Practice name required');
      setTimeout(() => {
        setErrortext2();
      }, 3000);
      return;
    }
    else {
      setErrortext2('')
    }
  }
  const handleSubmitPress = () => {
    //setErrortext('');
    if (!userEmail || !providerName || !practiceName) {
      alert('Please fill all fields');
      return;
    }

    fetch(global.url + "Practice/GetPracticeAndProvider?practice="
    + practiceName + "&provider=" +providerName, {
    method: "GET",
      headers: {
        'Content-Type': 'application/json,charset=utf-8',
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Credentials": true,
        'Accept': 'application/json, text/plain',
      }
      })
        .then((res) =>
        res.json()
        )
        .then((result) => {
          console.log(result)
          global.provider= result.User[0].userName;
          global.practice= result.User[0].practice;
          PatientLogin()
        })
      .catch((err) =>{
        //console.log(err))
        alert('Please check provider and practice');
        return;
      })
      
  }
  
    const PatientLogin = () => {
      fetch(global.url+'Practice/LoginPatient', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json,charset=utf-8',
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Credentials": true,
          'Accept': 'application/json, text/plain',
        },
        body: JSON.stringify({
          name: userEmail,
          url:  global.provider,
          practice:  global.practice,
          providerNameAttending:  global.provider,
        }),
        })
          .then((res) =>
          res.json()
          )
          .then((result) => {
            console.log(result)
            global.obj = result.User;
            global.token = result.Token;
            global.practice = result.User.practice,
            global.provider = result.User.providerNameAttending;
            navigation.replace('LiveVideoScreen');
          })
        .catch((err) =>
          console.log(err))
    }
    
 
  return (
    <View style={styles.mainBody}>
      {/* <Loader loading={loading} /> */}
      <ScrollView
        keyboardShouldPersistTaps="handled"
        contentContainerStyle={{
          flex: 1,
          justifyContent: 'center',
          alignContent: 'center',
        }}>
        <View>
          <KeyboardAvoidingView enabled>
            <View style={{ alignItems: 'center' }}>
              <Image
                source={require('../Image/aboutreact.png')}
                style={{
                  width: '50%',
                  height: 150,
                  resizeMode: 'contain',
                  margin: 30,
                }}
              />
            </View>
            <Text
              style={styles.registerTextStyle}
            >
              Please check in here to join your provider if you consent for this secure video visit.
            </Text>
            <View style={styles.SectionStyle}>
              <TextInput
                style={styles.inputStyle}
                onChangeText={(userEmail) =>
                  setUserEmail(userEmail)
                }
                placeholder="Enter your name"
                placeholderTextColor="#8b9cb5"
                autoCapitalize="none"
                keyboardType="default"
                returnKeyType="next"
                underlineColorAndroid="#f000"
                blurOnSubmit={false}
                onEndEditing={(e) => handlePatientValid(e.nativeEvent.text)}
              />
            </View>
            {errortext != '' ? (
              <Text style={styles.errorTextStyle}>
                {errortext}
              </Text>
            ) : null}

            <View style={styles.SectionStyle}>
              <TextInput
                style={styles.inputStyle}
                onChangeText={(providerName) =>
                  setProviderName(providerName)
                }
                placeholder="Enter provider name" //dummy@abc.com
                placeholderTextColor="#8b9cb5"
                autoCapitalize="none"
                keyboardType="default"
                returnKeyType="next"
                underlineColorAndroid="#f000"
                blurOnSubmit={false}
                onEndEditing={(e) => handleProviderValid(e.nativeEvent.text)}
              />
            </View>
            {errortext1 != '' ? (
              <Text style={styles.errorTextStyle}>
                {errortext1}
              </Text>
            ) : null}

            <View style={styles.SectionStyle}>
              <TextInput
                style={styles.inputStyle}
                onChangeText={(practiceName) =>
                  setPracticeName(practiceName)
                }
                placeholder="Enter practice name" //dummy@abc.com
                placeholderTextColor="#8b9cb5"
                autoCapitalize="none"
                keyboardType="default"
                returnKeyType="next"
                underlineColorAndroid="#f000"
                blurOnSubmit={false}
                onEndEditing={(e) => handlePracticeValid(e.nativeEvent.text)}
              />
            </View>
            {errortext2 != '' ? (
              <Text style={styles.errorTextStyle}>
                {errortext2}
              </Text>
            ) : null}
            <TouchableOpacity
              style={styles.buttonStyle}
              activeOpacity={0.5}
              onPress={handleSubmitPress}>
              <Text style={styles.buttonTextStyle}>Submit</Text>
            </TouchableOpacity>
          </KeyboardAvoidingView>
        </View>
      </ScrollView>
    </View>
  );
};
export default LoginScreen;

const styles = StyleSheet.create({
  mainBody: {
    flex: 1,
    justifyContent: 'center',
    backgroundColor: '#162550',
    alignContent: 'center',
  },
  SectionStyle: {
    flexDirection: 'row',
    height: 40,
    marginTop: 20,
    marginLeft: 35,
    marginRight: 35,
    margin: 10,
  },
  buttonStyle: {
    backgroundColor: '#3F51B5',
    borderWidth: 0,
    color: '#FFFFFF',
    borderColor: '#7DE24E',
    height: 40,
    alignItems: 'center',
    borderRadius: 30,
    marginLeft: 35,
    marginRight: 35,
    marginTop: 20,
    marginBottom: 25,
  },
  buttonTextStyle: {
    color: '#FFFFFF',
    paddingVertical: 10,
    fontSize: 16,
  },
  inputStyle: {
    flex: 1,
    color: 'white',
    paddingLeft: 15,
    paddingRight: 15,
    borderWidth: 1,
    borderRadius: 30,
    borderColor: '#dadae8',
  },
  registerTextStyle: {
    color: '#FFFFFF',
    textAlign: 'center',
    fontWeight: 'bold',
    fontSize: 14,
    alignSelf: 'center',
    padding: 10,
  },
  errorTextStyle: {
    color: 'white',
    textAlign: 'left',
    fontSize: 14,
    marginLeft: 40
  },
});