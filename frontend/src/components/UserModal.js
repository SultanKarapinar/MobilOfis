import React,{useState,useEffect} from 'react';
import {StyleSheet,ScrollView,View,Alert} from 'react-native';
import {Modal,Portal,TextInput,Button,Text,IconButton,HelperText} from 'react-native-paper';

const UserModal=({visible,onDismiss,OnRefresh,user})=>{
    const [form,setForm]=useState({
    name: "",
    role: "",
    email: "", 
    password: "" 
    });

    const [loading,setLoading]=useState(false);

    useEffect(()=>{
        if(user){
            setForm(user);
        }else{
            setForm({name: "", role: "", email: "", password: "" })
        }
    },[user,visible]);

    const handleSave=async()=>{
        if(!form.name)
            Alert.alert("Kullanıcı adı zorunludur.");
        return;
    
    setLoading(true);
    try{
        if(user){
            await api.put(`api/Users/${user.id}`,form);
            Alert.alert("Kullanıcı güncellendi")
        }else{
            await api.post(`api/Users`,form);
            Alert.alert("Kullanıcı eklendi.");
        }
        OnRefresh();
        onDismiss();
    }catch(error){
        Alert.alert("Bir hata oluştu.");
    }finally{
        setLoading(false);
    }};

    return (
    <Portal>
    <Modal visible={visible} onDismiss={onDismiss} contentContainerStyle={styles.container}>
        <View style={styles.header}>
            <Text style={styles.title}>{user ? "Kullanıcıyı Düzenle":"Yeni Kullanıcı"} </Text>
            <IconButton icon ="close" onPress={onDismiss}/>
        </View>
        <ScrollView>
            <TextInput
            label="Kullanıcı Adı"
            value={form.name}
            onChangeText={text =>setForm({...form,name:text})}
            mode="outlined"
            style={styles.input}/>
            <TextInput
            label="Email"
            value={form.email}
            onChangeText={text => setForm({...form,email:text})}
            mode="outlined"
            style={styles.input}/>

            <TextInput
            label="Rol"
            value={form.role}
            onChangeText={text=>setForm({...form,role:text})}
            mode="outlined"
            style={styles.input}/>
            <TextInput
            label="Şifre"
            value={"********" }
            editable={false}//yazılmasını sadece goruntulensın
            mode="outlined"
            style={styles.input}/>
            <HelperText type="info">
            Şifre güvenlik gereği bu ekrandan değiştirilemez.
            </HelperText>
            
            <Button
            mode="contained"
            onPress={handleSave}
            style={styles.button}>
                {user ? "Güncelle":"Kaydet"}
            </Button>
        </ScrollView>
    </Modal>
    </Portal>
);
};
const styles=StyleSheet.create({
     container:{
        backgroundColor:"#e3e8e9",
        padding:20,
        margin:20,
        borderRadius:12,
        maxHeight:"80%"
    },
    header:{
        flexDirection:"row",
        justifyContent:"space-between",
        alignItems:"center",
        marginBottom:10
    },
    title:{
        fontSize:18,
        fontWeight:"bold"
    },
    input:{
        marginBottom:12, 
        backgroundColor:"#f7ece7"
    },
    button:{
        marginTop:10,
        paddingVertical:5,
        backgroundColor:"#e7b79f"
    }
});
export default UserModal;
