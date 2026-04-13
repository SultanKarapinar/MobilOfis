import React, { useState } from 'react';
import { View, StyleSheet, KeyboardAvoidingView, Platform, ScrollView, Dimensions ,TouchableOpacity } from 'react-native';
// React Native Paper bileşenlerini kullanacağız (Daha profesyonel inputlar için)
import { TextInput, Button, Text, Surface } from 'react-native-paper'; 


const { width } = Dimensions.get('window');

const LoginScreen = ({ navigation }) => {

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [secureTextEntry, setSecureTextEntry] = useState(true); // Şifre gizleme

  const handleLogin = () => {

    navigation.navigate('Main');
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <ScrollView contentContainerStyle={styles.scrollContainer} keyboardShouldPersistTaps="handled">
      
        <Surface style={styles.loginCard} elevation={0}>
          
        
          <View style={styles.header}>
            <Text style={styles.logotext}>🕸️ Merkez Ofisim </Text>
            <Text style={styles.minText}>Tüm Ürünler Tek Merkez ` de</Text>
          </View>

      
          <View style={styles.form}>
           
            <TextInput
              label="Username"
              value={username}
              onChangeText={setUsername}
              mode="flat" //alttan çizgili
              style={styles.input}
              activeUnderlineColor="#000" // aktifken çizgi siyah olsun
              underlineColor="#a19a9a" // Normalde gri
              left={<TextInput.Icon icon="account-outline" color="#000" />} // kullanıcı ikonu
            />

            <TextInput
              label="Password"
              value={password}
              onChangeText={setPassword}
              mode="flat"
              style={styles.input}
              activeUnderlineColor="#000"
              underlineColor="#a19a9a"
              secureTextEntry={secureTextEntry} // Şifreyi göster
              left={<TextInput.Icon icon="lock-outline" color="#000" />} // kilit ikonu
              // sağ tarafa şifre gizle ikonu 
              right={
                <TextInput.Icon 
                  icon={secureTextEntry ? "eye-off-outline" : "eye-outline"} 
                  onPress={() => setSecureTextEntry(!secureTextEntry)} 
                  color="#666"
                />
              }
            />

            {/*logın button */}
            <Button
              mode="contained"
              onPress={handleLogin}
              style={styles.loginButton}
              labelStyle={styles.loginButtonText}
              buttonColor="#000" 
            >
              LOGIN
            </Button>
          </View>

          
          <View style={styles.footerLinks}>
            <TouchableOpacity>
            <Text style={styles.linkText} onPress={() => console.log('Forgot Password')}>
              Forgot your password?
            </Text>
            </TouchableOpacity>
          
          </View>
  
        </Surface>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#cfe0bb', // arka plan
  },
  scrollContainer: {
    flexGrow: 1,
    justifyContent: 'center', // İçeriği dikeyde ortala
    padding: 20,
    minHeight: 100,
  },
  loginCard: {
    backgroundColor:'#cfe0bb',
    padding: 20,
    alignItems: 'center',
    width: '100%',
  },
  header: {
    marginBottom: 60, // Form ile başlık arasına boşluk
  },
  logotext: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#000',
    letterSpacing: 1.5, // Harf arası boşluk kurumsal gösterir
  },
  minText:{
    fontsize:22,
    marginLeft:40,
  },
  form: {
    width: '100%',
    alignItems: 'center',
  },
  input: {
    width: '100%',
    backgroundColor: '#cfe0bb', // Arka planla aynı olsun
    marginBottom: 20,
    fontSize: 16,
  },
  loginButton: {
    marginTop: 30,
    width:100, // Biraz daha dar ve dikdörtgen
    borderRadius: 8, 
    height: 50,
    justifyContent: 'center',
  },
  loginButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
    letterSpacing: 1,
  },
  footerLinks: {
    flexDirection: 'row',
    justifyContent: 'center',
    width: '100%',
    marginTop: 60,
  },
  linkText: {
    fontSize: 12,
    color: '#666',
    fontWeight: '500',
  },
});

export default LoginScreen;