import React from 'react';
import {View,Text,StyleSheet} from 'react-native';

const EmailNotificationSentScreen =()=>{
    return(
        <View style={styles.center}>
            <Text style={styles.text}>Gönderilen E-Postalar</Text>
        </View>
    );
};
const styles=StyleSheet.create({
 center: {flex:1, justifyContent:'center', alignItems:'center'},
 text:{fontsize:20,color:'#786'}
});
export default EmailNotificationSentScreen;
