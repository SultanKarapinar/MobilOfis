import React ,{useState,useEffect} from 'react';
import { StyleSheet ,ScrollView,View, Alert } from 'react-native';
import { Modal,Portal,TextInput, Button,Text,IconButton } from 'react-native-paper';
import api from '../services/api';

const SupplierModal = ({visible,onDismiss,onRefresh,supplier})=>{
    const [form,setForm] =useState({
        name:"",
        taxNumber:"",
        phone:"",
        email:"",
        address:""
    });

    const [loading,setLoading]=useState(false);

    useEffect(()=>{
        if(supplier){
            setForm(supplier);
        }else{
            setForm({name:"",taxNumber:"",phone:"",email:"",address:""});
        }
    },[supplier,visible]);

    const handleSave=async ()=>{
        if(!form.name)
        {//formdaı bos s ıse false olur unlemle bu true olur 
            //ve ıf ın ıcıne gırer
            Alert.alert("Tedarikçi adı zorunludur.");
            return;
        }
        setLoading(true);
        try {
            if(supplier){
                await api.put(`api/Suppliers/${supplier.id}`,form);
                Alert.alert("Tedarikçi güncellendi")
            }else{
                await api.post(`api/Suppliers`,form);
                Alert.alert("Tedarikçi eklendi.");
            }
            onRefresh();
            onDismiss();
        } catch (error) {
            Alert.alert("Bir hata oluştu.");
        }finally{
            setLoading(false);
        }
    };
    return(
        <Portal>
            <Modal visible={visible} onDismiss={onDismiss} contentContainerStyle={styles.container}>
               <View style={styles.header}>
                <Text style={styles.title}>{supplier ? "Tedarikçiyi Düzenle" : "Yeni Tedarikçi"}</Text>
                <IconButton icon="close" onPress={onDismiss}/>
                </View> 
                <ScrollView>
                    <TextInput
                    label="Tedarikçi Adı"
                    value={form.name}
                    onChangeText={text =>setForm({...form,name:text})}
                    mode="outlined"
                    style={styles.input}/>

                    <TextInput
                    label="Email"
                    value={form.email}
                    onChangeText={text =>setForm({...form,email:text})}
                    mode="outlined"
                    keyboardType="email-address"
                    style={styles.input}
                    />

                    <TextInput
                    label="Telefon"
                    value={form.phone}
                    onChangeText={text =>setForm({...form,phone:text})}
                    mode="outlined"
                    keyboardType="phone-pad"
                    style={styles.input}
                   />
                   <TextInput
                   label="Vergi Numarası"
                   value={form.taxNumber}
                   onChangeText={text =>setForm({...form,taxNumber:text})}
                   mode="outlined"
                   style={styles.input}
                   />
                   <TextInput
                   label="Adres"
                   value={form.address}
                   onChangeText={text=>setForm({...form,address:text})}
                   mode="outlined"
                   multiline
                   numberOfLines={3}
                   style={styles.input}
                   />
                   <Button
                   mode="contained"
                   onPress={handleSave}
                   style={styles.button}
                   >
                    {supplier ? "Güncelle" : "Kaydet"}
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
        backgroundColor:"#c7dbe6"
    },
    button:{
        marginTop:10,
        paddingVertical:5,
        backgroundColor:"#6ca6c7"
    }
});
export default SupplierModal;