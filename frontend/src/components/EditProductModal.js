 import React, { useState, useEffect } from 'react';
import { StyleSheet, ScrollView, View } from 'react-native';
import { Modal, Portal, TextInput, Button, Text, IconButton } from 'react-native-paper';
import { Dropdown } from 'react-native-element-dropdown';
import api from '../services/api';

const EditProductModal = ({ visible, onDismiss, onRefresh, product}) => {
    const[updateFrom,setUpdateForm]=useState({
    name: "",
    categoryId: null,
    currentStock: "",
    reorderLevel: "",
    unitOfMeasure: ""
    });
    const [categories,setCategories]=useState([]);
   
    const [loading, setLoading] = useState(false);

  
    const unitOptions = [
        { label: 'Kg', value: 0 },
        { label: 'Paket', value: 1 },
        { label: 'Litre', value: 2 },
        { label: 'Adet', value: 3 },
    ];

   
 useEffect(() => {
    if (visible && product) {
        setUpdateForm({
            name: product.name || "",
            categoryId: product.categoryId || null,
            currentStock: product.currentStock || 0,
            reorderLevel: product.reorderLevel || 0,
            unitOfMeasure: product.unitOfMeasure ?? 0
        });

        listCategories();
    }
}, [visible, product]);

   const listCategories = async () => {
    try {
        const rest = await api.get("/api/Category");

        setCategories(
            rest.data.map(x => ({
                label: x.name,
                value: x.id
            }))
        );

    } catch (error) {
        console.error("Kategoriler yüklenemedi", error);
    }
};

    const handleUpdate = async () => {
     
        setLoading(true);
        try {
          
           await api.put(`/api/Products/${product.id}`, updateFrom);

            alert("Ürün başarıyla güncellendi");
            onRefresh();
            onDismiss(); 
        } catch (error) {
            console.error(error);
            alert("Güncelleme sırasında hata oluştu.");
        } finally {
            setLoading(false);
        }
    };
    

    return (
        <Portal>
            <Modal visible={visible} onDismiss={onDismiss} contentContainerStyle={styles.container}>
                <View style={styles.header}>
                    <Text style={styles.title}>Ürünü Düzenle</Text>
                    <IconButton icon="close" onPress={onDismiss} />
                </View>

                <TextInput
                    label="Ürün Adı"
                    value={updateFrom.name}
                    onChangeText={text=>setUpdateForm(prev=>({...prev,name:text}))}
                    mode="outlined"
                    style={styles.input}
                />

                   {/* <Text style={styles.label}>Kategori</Text> */}
                <Dropdown
                    style={styles.dropdown}
                    data={categories}
                    labelField="label"
                    valueField="value"
                  // placeholder="Birim Seçin"
                    value={updateFrom.categoryId}
                   onChange={item =>
  setUpdateForm(prev => ({
    ...prev,
    categoryId: item.value
  }))
}
                />

                {/* <Text style={styles.label}>Birim</Text> */}
                <Dropdown
                    style={styles.dropdown}
                    data={unitOptions}
                    labelField="label"
                    valueField="value"
                   // placeholder="Birim Seçin"
                    value={updateFrom.unitOfMeasure}
                    onChange={item =>
  setUpdateForm(prev => ({
    ...prev,
    unitOfMeasure: item.value
  }))
}
                />
               {/* <Text style={styles.label}>Güncel Stok Seviyesi</Text> */}
                 <TextInput
                    label="Güncel Stok Seviyesi"
                    value={updateFrom.currentStock.toString()}
                  onChangeText={text =>
  setUpdateForm(prev => ({
    ...prev,
    reorderLevel: text.replace(/[^0-9]/g, '')
  }))
}
                    mode="outlined"
                    keyboardType="numeric"
                    style={styles.input}/>

                {/* <Text style={styles.label}>Min Stok Seviyesi</Text> */}
                                    <TextInput
                                    label="Min Stok Seviyesi"
                                    value={updateFrom.reorderLevel.toString()}
                                    onChangeText={text=>updateFrom(prev=>({...prev,reorderLevel:text.replace(/[^0-9]/g, '') }))}
                                    mode="outlined"
                                    keyboardType="numeric"
                                    style={styles.input}/>    

                <Button 
                    mode="contained" 
                    onPress={handleUpdate} 
                    loading={loading}
                    style={styles.button}
                >
                    Güncelle
                </Button>
            </Modal>
        </Portal>
    );
};

const styles = StyleSheet.create({
    container: { 
        backgroundColor: '#e3e6e2',
         padding: 20,
          margin: 20, 
         borderRadius: 12 
        },
    header: {
         flexDirection: 'row',
          justifyContent: 'space-between',
           alignItems: 'center',
            marginBottom: 15 
        },
    title: { 
        fontSize: 20,
         fontWeight: 'bold'
         },
    input: {
         marginBottom: 15 
        },
    label: { 
        fontSize: 14, 
        color: '#666',
         marginBottom: 5 },
    dropdown: {
        height: 50,
        borderColor: '#0e0d0d',
        borderWidth: 1,
        borderRadius: 5,
        paddingHorizontal: 10,
        marginBottom: 20
    },
    button: { paddingVertical: 5 }
});

export default EditProductModal;