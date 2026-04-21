import React,{useEffect,useState} from 'react';
import { StyleSheet,View , FlatList,ScrollView,Alert } from 'react-native';
import api from '../services/api';
import CategoryModal from '../components/CategoryModal';

import { FAB,Card ,Text ,IconButton,Divider,ActivityIndicator} from 'react-native-paper';

const CategoryScreen = ()=>{
 const [categories,setCategories]=useState([]);
 const [products,setProducts]=useState({});
 const[expandedId,setExpandedId]=useState(null);//hangı  kategorının acxık oldugunu tutar
 const [loading,setLoading]=useState(true);
 const[modalVisible,setModalVisible]=useState(false);
 const[selectedCategory,setSelectedCategory]=useState(null);

 useEffect(()=>
{
  listCategories();
},[]);

const listCategories = async ()=>{
 setLoading(false);
 try {
  const res= await api.get(`api/Category`);
  setCategories(res.data);
 } catch (error) {
  console.error(error);
 }

};

const listProductsByCategory=async(categoryId)=>{
 if(expandedId === categoryId){
  setExpandedId(null);//zaten acıksa kaapt
  return;
 }
 try{
  const res=await api.get(`api/Category/${categoryId}/products`);
  setProducts(prev=>({...prev,[categoryId]: res.data}));
  setExpandedId(categoryId)//kategorı ac
 }catch(error)
 {
  Alert.alert("Hata", "Ürünler Yüklenemedi.");
 }
};

const handleDelete=(id)=>{
  Alert.alert("Emin misiniz?","Kategoriye baglı ürün varsa silme işlemi başarısız olabilir.",[
    {text:"İptal",style:"cancel"},
    {text:"Sil",style:"destructive",onPress:async ()=>
    {
      try {
        await api.delete(`api/Category/${id}`);
        setCategories(prev=>prev.filter(c => c.id !== id));
        Alert.alert("Başarılı","Kategori Silindi.")
        
      } catch (error) {
        Alert.alert("Hata","Kategori Silinemedi.")
      }
    }
    }
  ]);
};

const  renderCategoryItem=({item})=>(
  <Card style={styles.card} onPress={()=>listProductsByCategory(item.id)}>
    <Card.Content>
      <View style={styles.cardHeader}>
        <View>
          <Text  variant="titleMedium" style={styles.categoryName}>{item.name}</Text>
          <Text variant="bodySmall"> ID:{item.id}</Text>
          {/* variant:texttekı yazı tıpını boyutunu belırledı */}
        </View>
        <View style={styles.actions}>
          <IconButton icon="pencil" size={20} iconColor="#0e0d0d" onPress={() => {
                            setSelectedCategory(item);
                            // burada id almayarak modele duzenleme modalını ac demıs oluyor
                            setModalVisible(true);
                        }} />
          <IconButton icon="trash-can" size={20} iconColor="#0f0f0f" onPress={()=>handleDelete(item.id)}/>
        </View>
      </View>
      {expandedId === item.id && (
        <View style={styles.productList}>
          <Divider style={styles.divider}/>
          {/* divider ekranda ince ayırıcı çizgi çizer */}
          <Text style={styles.productTitle}>Kategoriye Ait Ürünler:</Text>
          {products[item.id]?.length > 0 ?(
            products[item.id].map(p => (
              <Text key={p.id} style={styles.productItem}>•{p.name} </Text>
            ))
          ):(
            <Text style={styles.noProduct}>Bu kategoride ürün bulunamadı.</Text>
          )}
        </View>
      )}

    </Card.Content>
  </Card>
);
return (
  <View style={styles.container}>
    {loading ? (<ActivityIndicator style={{flex:1}}/>)
    :(
      <FlatList
      data={categories}
      keyExtractor={(item)=>item.id.toString()}
      renderItem={renderCategoryItem}
      contentContainerStyle={styles.listContent}/>
    )}
    <FAB
    icon="plus"
    style={styles.fab}
    onPress={()=>{
      setSelectedCategory(null);
      setModalVisible(true);
    }}
    />
    <CategoryModal
    visible={modalVisible}
    onDismiss={()=>setModalVisible(false)}
    onRefresh={listCategories}
    category={selectedCategory} //buradaki category modeldeki parametre buna gore nodel ekleme ve duxenleme yapıyor 
    />

  </View>
);



};
const styles=StyleSheet.create({
  container:{
    flex:1,
    backgroundColor:'#fafafa'
  },
  listContent:{
    padding:10,
    elevation:2
  },
  card:{
    marginBottom:10,
    elevation:2,
    backgroundColor:"#ccbece"
   // backgroundColor:"#dad8d8"
  },
  cardHeader:{
    flexDirection:'row',
    justifyContent:'space-between',
    alignItems:'center',
   // backgroundColor:"#e8e9e8"
  },
  actions:{
    flexDirection:'row'
  },
  categoryName:{
    fontWeight:'bold',
    color:'#080808'
  },
  productList:{
    marginTop:10,
    //backgroundColor:'#f3f3f1',
    padding:8,
    borderRadius:5
  },
  divider:{
    marginVertical:10,
    backgroundColor:"#080808"
  },
  productTitle:{
    fontWeight:'bold',
    marginBottom:5,
    fontSize:15
  },
  productItem:{
    paddingVertical:2,
    color:'#0e0d0d'
  },
  noProduct:{
    fontStyle:'italic',
    color:'gray'
  },
  fab:{
    position:'absolute',
    margin:16,
    right:10,
    bottom:40,
    backgroundColor:"#B5A0B8"
  }
  
});
export default CategoryScreen;