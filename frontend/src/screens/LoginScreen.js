import React, { useState, useRef, useEffect , useContext} from 'react';
import { 
  View, StyleSheet, KeyboardAvoidingView, Platform, ScrollView, 
  Dimensions, TouchableOpacity, Alert, Modal 
} from 'react-native';
import { TextInput, Button, Text, Surface, HelperText } from 'react-native-paper';
import { jwtDecode } from "jwt-decode";
import api from '../services/api';
import { AuthContext } from '../context/AuthContext';
import {setToken as saveToken } from '../services/tokenService';

const LoginScreen = ({ navigation }) => {
const { login } = useContext(AuthContext);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [secureTextEntry, setSecureTextEntry] = useState(true);
  const [loading, setLoading] = useState(false);
  const passwordRef = useRef(null);

  const [showModal, setShowModal] = useState(false);
  const [resetStep, setResetStep] = useState(1);
  const [resetEmail, setResetEmail] = useState("");
  const [resetCode, setResetCode] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [timeLeft, setTimeLeft] = useState(180);
  const [timerActive, setTimerActive] = useState(false);


  const [alertMessage, setAlertMessage] = useState(null);

  useEffect(() => {
    if (alertMessage) {
      Alert.alert(alertMessage.title, alertMessage.message);
      setAlertMessage(null);
    }
  }, [alertMessage]);

  useEffect(() => {
    let interval; //sureyı tutar 
    if (timerActive && timeLeft > 0) {
      interval = setInterval(() => {
        setTimeLeft((prev) => prev - 1);
      }, 1000);
    } else if (timeLeft === 0) {
      setTimerActive(false);
      setResetStep(1);

 
      setAlertMessage({
        title: "Süre Doldu",
        message: "Lütfen yeni bir kod talep edin."
      });
    }
    return () => clearInterval(interval);
  }, [timerActive, timeLeft]);

  const handleLogin = async () => {
    if (!username || !password) {
      setAlertMessage({
        title: "Hata",
        message: "Lütfen kullanıcı adı ve şifrenizi giriniz."
      });
      return;
    }

    setLoading(true);

    try {
      const response = await api.post("api/auth/login", {
        Username: username,
        password: password
      });

      const token = response.data.token;

      const decoded = jwtDecode(token);
    const userObj = {
  id: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || decoded.sub,
  name: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || username,
  role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || "Kullanıcı"
};

login(userObj, token);
saveToken(token);
      setLoading(false);

      navigation.navigate('Main');

    } catch (err) {
      setLoading(false);

      const msg = err.response?.data?.message || "Kullanıcı adı veya şifre hatalı.";


      setAlertMessage({
        title: "Giriş Başarısız",
        message: msg
      });
    }
  };

  const handleSendCode = async () => {
    try {
      await api.post("api/auth/forgot-password", JSON.stringify(resetEmail), {
        headers: { 'Content-Type': 'application/json' }
      });
      setResetStep(2);
      setTimerActive(true);
    } catch (err) {
      setAlertMessage({
        title: "Hata",
        message: "Kod gönderilemedi."
      });
    }
  };

  const handleResetPassword = async () => {
    if (newPassword !== confirmPassword) {
      setAlertMessage({
        title: "Hata",
        message: "Şifreler uyuşmuyor"
      });
      return;
    }

    try {
      await api.post("api/auth/reset-password", {
        Email: resetEmail,
        Code: resetCode,
        NewPassword: newPassword,
        ConfirmPassword: confirmPassword 
      });

      setShowModal(false);

     
      setTimeout(() => {
        setAlertMessage({
          title: "Başarılı",
          message: "Şifreniz güncellendi."
        });
      }, 300);

    } catch (err) {
      setShowModal(false);

      setTimeout(() => {
        setAlertMessage({
          title: "Hata",
          message: "Kod yanlış veya süresi dolmuş."
        });
      }, 300);
    }
  };

  return (
    <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'} style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContainer}>
        <Surface style={styles.loginCard}>
          <View style={styles.header}>
            <Text style={styles.logotext}>🕸️ MERKEZ OFİSİM</Text>
            <Text style={styles.minText}>Tüm Ürünler Tek MERKEZ'de</Text>
          </View>

          <View style={styles.form}>
            <TextInput
              label="Kullanıcı Adı"
              value={username}
              onChangeText={setUsername}
              mode="flat"
              style={styles.input}
              returnKeyType="next"
              onSubmitEditing={() => passwordRef.current.focus()} // Web'deki Enter mantığı
              left={<TextInput.Icon icon="account" />}
            />

            <TextInput
              label="Şifre"
              value={password}
              ref={passwordRef}
              onChangeText={setPassword}
              secureTextEntry={secureTextEntry}
              mode="flat"
              style={styles.input}
              left={<TextInput.Icon icon="lock" />}
              right={<TextInput.Icon icon={secureTextEntry ? "eye-off" : "eye"} onPress={() => setSecureTextEntry(!secureTextEntry)} />}
            />

            <Button mode="contained" loading={loading} onPress={handleLogin} style={styles.loginButton} buttonColor="#000">
              GİRİŞ YAP
            </Button>

            <TouchableOpacity onPress={() => setShowModal(true)} style={{ marginTop: 20 }}>
              <Text style={styles.linkText}>Şifremi unuttum?</Text>
            </TouchableOpacity>
          </View>
        </Surface>

        {/* ŞİFRE SIFIRLAMA MODALI */}
        <Modal visible={showModal} animationType="slide" transparent={true}>
          <View style={styles.modalOverlay}>
            <Surface style={styles.modalContent}>
              <TouchableOpacity style={styles.closeBtn} onPress={() => setShowModal(false)}>
                <Text style={{ fontSize: 20 }}>✕</Text>
              </TouchableOpacity>

              {resetStep === 1 ? (
                <>
                  <Text style={styles.modalTitle}>Şifre Sıfırlama</Text>
                  <TextInput 
                    label="E-posta" 
                    value={resetEmail} 
                    onChangeText={setResetEmail} 
                    mode="outlined" 
                    style={styles.modalInput} 
                  />
                  <Button mode="contained" onPress={handleSendCode} buttonColor="#000">KOD GÖNDER</Button>
                </>
              ) : (
                <>
                  <Text style={styles.modalTitle}>Yeni Şifre</Text>
                  <Text style={{ color: timeLeft < 30 ? 'red' : 'green', textAlign: 'center' }}>
                    Kalan Süre: {Math.floor(timeLeft / 60)}:{(timeLeft % 60).toString().padStart(2, '0')}
                  </Text>
                  <TextInput label="6 Haneli Kod" value={resetCode} onChangeText={setResetCode} mode="outlined" style={styles.modalInput} maxLength={6} />
                  <TextInput label="Yeni Şifre" value={newPassword} onChangeText={setNewPassword} secureTextEntry mode="outlined" style={styles.modalInput} />
                  <TextInput label="Şifre Tekrar" value={confirmPassword} onChangeText={setConfirmPassword} secureTextEntry mode="outlined" style={styles.modalInput} />
                  {newPassword !== confirmPassword && <HelperText type="error">Şifreler uyuşmuyor!</HelperText>}
                  <Button mode="contained" onPress={handleResetPassword} buttonColor="#000">GÜNCELLE</Button>
                </>
              )}
            </Surface>
          </View>
        </Modal>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: {
     flex: 1, 
     backgroundColor: '#cfd3ca'
     },
  scrollContainer: { 
    flexGrow: 1, 
    justifyContent: 'center',
     padding: 20
     },
  loginCard: {
     backgroundColor: '#c0caca',
      padding: 30,
       borderRadius: 15,
        alignItems: 'center'
       },
  header: {
     marginBottom: 40, 
     alignItems: 'center' 
    },
  logotext: {
     fontSize: 24,
      fontWeight: 'bold', 
      letterSpacing: 1 
    },
  minText: { 
    fontSize: 14, 
    color: '#555'
   },
  form: { 
    width: '100%'
   },
  input: { 
    marginBottom: 15, 
    backgroundColor: 'transparent'
   },
  loginButton: { 
    marginTop: 10, 
    borderRadius: 5 
  },
  linkText: { 
    textAlign: 'center', 
    color: '#333',
     fontWeight: 'bold' 
    },
  // Modal Stilleri

  modalOverlay: { 
    flex: 1,
     backgroundColor: 'rgba(0,0,0,0.5)',
      justifyContent: 'center', 
      padding: 20 
    },
  modalContent: {
     padding: 25, 
     borderRadius: 10, 
     backgroundColor: '#fff'
     },
  modalTitle: {
     fontSize: 20, 
     fontWeight: 'bold', 
     marginBottom: 15,
      textAlign: 'center'
     },
  modalInput: { 
    marginBottom: 10 
  },
  closeBtn: { 
    alignSelf: 'flex-end',
     padding: 5 
    }
});

export default LoginScreen;