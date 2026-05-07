import React ,{useState,useEffect} from 'react';
import { StyleSheet,View ,ScrollView ,Alert} from 'react-native';
import { Modal,Portal, TextInput,Button, Text,IconButton,SegmentedButtons ,HelperText} from 'react-native-paper';
//portal:modalı herseyın ustunde gorunmes ıcın
//segmentedButtons: radıo button gıbı tek secım
//helperınput :ınputun altındakı kucuk rehber 
import { Dropdown } from 'react-native-element-dropdown';
import api from '../services/api';

const AddProductModal=({visible,onDismiss,onRefresh})=>{
    const [addFrom, setAddFrom]=useState({
        name:"",
        categoryId:null,
        reorderLevel:"",
        unitOfMeasure:""

    });
    const [categories,setCategories]=useState([]);
    const [loading,setLoading]=useState(false);

    const unitOptions=[
        {label:"Kg",value:0},
        {label:"Paket",value:1},
        {label:"Litre",value:2},
        {label:"Adet",value:3}
    ];
    useEffect(()=>{
        if(visible)
        {listCategories()}
    },[visible]);

    const listCategories=async()=>{
        try{
            const rest=await api.get("/api/Category");
            setCategories(rest.data);
        }catch(error)
        {
            console.error("Kategoriler yüklenemedi",error);
        }
    };

    const handleAddproduct=async ()=>{
        if(!addFrom.name|| !addFrom.categoryId)
        {
           Alert.alert("Uyarı", "Lütfen ürün adı ve kategori seçiniz.");
            return;
        }
        setLoading(true);
        try{
            await api.post("/api/Products",addFrom);
            Alert.alert("Yeni Ürün Eklendi");
            onRefresh();
            handleClose();
        }catch(error)
        {
            if (error.response && (error.response.status===403 ||error.response.status===401))
            {
                Alert.alert("Erişim İzniniz Yok!");
            }else{
                Alert.alert("Ürün Eklenirken Bir Hata Oluştu.");
            }
        }
        console.log(token);
    };
     const handleClose=()=>
     {
        setAddFrom({name:"",categoryId:null,reorderLevel:"",unitOfMeasure:""});
        onDismiss();
     };
     return (
        <Portal>
            <Modal visible={visible} onDismiss={handleClose} contentContainerStyle={styles.modal}>
                <ScrollView showsVerticalScrollIndicator={false}>
                    <Text style={styles.title}>Yeni Ürün Ekle</Text>
                     {/* <IconButton icon="close" onPress={onDismiss} /> */}
                    <Text style={styles.label}>Ürün Adı</Text>
                    <TextInput
                    label="Ürün Adı"
                    value={addFrom.name}
                    onChangeText={text=>setAddFrom(prev=>({...prev,name:text}))}
                    mode="outlined"
                    style={styles.input}/>

                    <Text style={styles.label}>Kategori</Text>
                    <Dropdown
                    style={styles.dropdown}
                      containerStyle={{ backgroundColor: '#e4e3dd' }}
itemContainerStyle={{ borderRadius: 8 }}
                    placeholderStyle={styles.placeholderStyle}
                    selectedTextStyle={styles.selectedTextStyle}
                    data={categories}
                    maxHeight={300}
                    labelField="name"
                    valueField="id"
                    placeholder="Kategori Seçiniz"
                    value={addFrom.categoryId}
                    onChange={item=>setAddFrom(prev=>({...prev,categoryId:item.id }))}   
                    />
                    <Text style={styles.label}>Birim</Text>
                   <Dropdown
  style={styles.dropdown}
  containerStyle={{ backgroundColor: '#e4e3dd' }}
itemContainerStyle={{ borderRadius: 8 }}
  placeholderStyle={styles.placeholderStyle}
  selectedTextStyle={styles.selectedTextStyle}
  data={unitOptions}
  labelField="label"
  valueField="value"
  placeholder="Birim Seçiniz"
  value ={addFrom.unitOfMeasure}
  onChange={item =>
    setAddFrom(prev => ({
      ...prev,
      unitOfMeasure: item.value,
    }))
  }
/>
                    <Text style={styles.label}>Min Stok Seviyesi</Text>
                    <TextInput
                    label="Min Stok Seviyesi"
                    value={addFrom.reorderLevel.toString()}
                    onChangeText={text=>setAddFrom(prev=>({...prev,reorderLevel:text.replace(/[^0-9]/g, '') }))}
                    mode="outlined"
                    keyboardType="numeric"
                    style={styles.input}/>
                    <View style={styles.actionRow}>
                    <Button mode="contained"
                    onPress={handleAddproduct}
                    loading={loading}
                    style={styles.btn}>Kaydet</Button>
                    <Button onPress={handleClose}
                    style={styles.btn}> İptal </Button>

                    </View>
                </ScrollView>
            </Modal>
        </Portal>
     );
};
const styles=StyleSheet.create({
    modal:{
        backgroundColor:"#e3e6e2",
        padding:20,
        margin:20,
        borderRadius:15,
        maxHeight:'90%'
    },
    title:{
        fontsize:22,
        fontWeight:'bold',
        marginBottom:20,
        textAlign:'center',
        color:'#5f779e'
    },
    label:{
        fontSize:14,
        color:'#666',
        marginBottom:5,
        marginTop:10
    },
    input:{
        marginBottom:10,
       // backgroundColor:'#FFF8EC'
    },
    dropdown:{
        height:50,
        borderColor:'#0e0d0d',
        borderWidth:1,
        borderRadius:5,
        paddingHorizontal:10,
        marginBottom:10,
       //  backgroundColor:'#FFF8EC'
       
       
    },
    placeholderStyle:{
        fontSize:16,
        
    },
    selectedTextStyle:{
        fontSize:16,
      //   backgroundColor:'#FFF8EC'
        
    },
    segmented:{
        marginBottom:15,
        color:'#A1BC98'
        
        
       
    },
    actionRow:{
        marginTop:20,
        flexDirection:'column',
        gap:10,
       
    },
    btn:{
        paddingVertical:4,
       
    },


});
export default AddProductModal;