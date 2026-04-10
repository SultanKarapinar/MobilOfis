import React from 'react';
import { StyleSheet,Text, View } from 'react-native';

const EmailNotificationSettingScreen=()=>{
    return(
            <View style={styles.center}>
        <Text style={styles.text}> E- posta ayarları ekranı</Text>
    </View>
    );

};
 const styles=StyleSheet.create
({
  center:{flex:1,justifyContent:'center',alignItems:'center'},
  text:{fontSize:20,color:'#1582'}
});
export default EmailNotificationSettingScreen;

