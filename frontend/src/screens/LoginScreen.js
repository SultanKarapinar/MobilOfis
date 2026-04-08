import React, { useState } from 'react';

import { 
  StyleSheet, 
  Text, 
  View, 
  TextInput, 
  TouchableOpacity, 
  KeyboardAvoidingView, 
  Platform,
  Image
} from 'react-native';

const LoginScreen = ({navigation}) => {
  const [kullanıcıAdı, setKullanıcıAdı] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = () => {
    navigation.navigate('Main')
   // console.log("Giriş denemesi:", email, password);
    // Buraya ileride C# API bağlantısını (Axios) ekleyeceğiz.
  };

  return (
    <KeyboardAvoidingView 
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >{/*klavye acıldıgında içeriklerin yukarı kaymasını saglar  */}
      <View style={styles.innerContainer}>
        {/* Logo Alanı (Opsiyonel) */}
        <Text style={styles.logoText}>Merkez Ofisim</Text>
        <Text style={styles.subTitle}>"Tüm Ürünler Tek Merkez ' de"</Text>

        <View style={styles.inputContainer}>
          <TextInput
            style={styles.input}
            placeholder="Kullanıcı Adı"
            value={kullanıcıAdı}
            autoFocus={true}
            onChangeText={setKullanıcıAdı}
            // keyboardType="email-address"
            autoCapitalize="none"
          />
          
          <TextInput
            style={styles.input}
            placeholder="Şifre"
            value={password}
            onChangeText={setPassword}
            secureTextEntry // Şifreyi yıldızlı gösterir
          />
        </View>

        <TouchableOpacity style={styles.button} onPress={handleLogin}>
          <Text style={styles.buttonText}>Giriş Yap</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.forgotPassword}>
          <Text style={styles.forgotText}>Şifremi Unuttum</Text>
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#cfe0bb',
   
  },
  innerContainer: {
    flex: 1,
    justifyContent: 'center',
    paddingHorizontal: 30,
  },
  logoText: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#474d55',
    textAlign: 'center',
  },
  subTitle: {
    fontSize: 16,
    color: '#68745c',
    textAlign: 'center',
    marginBottom: 40,
  },
  inputContainer: {
    marginBottom: 20,
  },
  input: {
    backgroundColor: '#f5f5f5',
    paddingHorizontal: 15,
    paddingVertical: 12,
    borderRadius: 10,
    marginBottom: 15,
    borderWidth: 1,
    borderColor: '#ddd',
  },
  button: {
    backgroundColor: '#5b8168',
    paddingVertical: 15,
    borderRadius: 10,
    alignItems: 'center',
    elevation: 2,
  },
  buttonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
  },
  forgotPassword: {
    marginTop: 20,
    alignItems: 'center',
  },
  forgotText: {
    color: '#e90909',
    fontWeight: '600',
  },
});

export default LoginScreen;