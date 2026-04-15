import React ,{useState} from 'react';
import {StyleSheet} from 'react-native';
import{Modal,Portal,TextInput,Button,Text} from 'react-native-paper';
 import api from '../services/api';


 const AddProductModal=({visible,onDismiss,onRefresh})=>{
    const [name,setName]=useState('');
    const [categoryId,setCategoryId]=useState('');
    const [ reorderLevel,setReorderLevel]=useState('');
    const [ unitOfMeasure,setUnitOfMeasure]=useState('');
  
    const addProduct=async ()=>
    {
        try{
            const newProduct={
                name:name,
                categoryId:categoryId,
                reorderLevel:reorderLevel,
                unitOfMeasure:unitOfMeasure
            }
            await api.post('/Product',newProduct);
            onRefresh();//yenılemek fonksıyonu cagır
            onDismiss();//modalı kapatır
            setName('');setCategoryId('');setReorderLevel('');setUnitOfMeasure(''); //form temızle
        }
        catch(error)
        {
           console.error ("kayıt hatası",error)
        }
    };

    return (
       <Portal>
        <Modal visible={visible} onDismiss={onDismiss}contentContainerStyle ={styles.modal}>
        <Text style={styles.title}>Yeni Ürün Ekle</Text>
        <TextInput label="Ürün Adı"value={name} onChangeText={setName} mode="outlined"/>
        
        </Modal>
       </Portal> 
    )

 }