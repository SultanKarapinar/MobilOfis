import React ,{useEffect,useState} from 'react';
import { StyleSheet , View,FlatList,Alert} from 'react-native';
import {Card,Text,FAB,IconButton,Divider,ActivityIndicator,List} from 'react-native-paper';
import api from '../services/api';
import SupplierModal from '../components/SupplierModal';

const SupplierScreen= ()=>{
  const [suppliers,setSuppliers]=useState([]);
  const [loading,setLoading]=useState(false);
  const [expandedId,setExpandedId]=useState(null);
  const [modalVisible,setModalVisible]=useState(false); 
  const [selectedSupplier,setSelectedSupplier]=useState(null);

  useEffect (()=>{
    listSuppliers();
  },[]);

  const listSuppliers= async ()=>{
    setLoading(true);
    try {
      const res=await api.get(`api/Suppliers`);
      setSuppliers(res.data);
    } catch (error) {
      console.error(error);
      Alert.alert("Hata","Tedarikçiler yüklenemedi.");
    }finally{
      setLoading(false);
    }
  };

  const handleDelete=(id)=>{
    Alert.alert("Emin misiniz?","Tedarikçi kaydı tamanen silinecektir.",[
      {text:"İptal",style:"cancel"},
      {text:"Sil",style:"destructive",onPress: async ()=>{
        try{
          await api.delete(`api/Suppliers/${id}`);
          setSuppliers(prev=>prev.filter(s=> s.id !==id));
        }catch(err){
          Alert.alert("Hata","Erişim izniniz yok veya bir hata oldu.");
        }
      } }
    ]);
  };
  const renderItem=({item})=>(
    <Card
    style={styles.card}
    onPress={()=>setExpandedId(expandedId===item.id ? null:item.id)}>
      <Card.Content>
        <View style={styles.cardHeader}>
          <View style={styles.titleGroup}>
            <IconButton icon="domain" size={24} iconColor="#397c91"/>
            <View>
              <Text variant="titleMedium" style={styles.suppliersName}>{item.name}</Text>
              <Text variant="bodySmall">ID:{item.id}</Text>
            </View>
          </View>
          <View style={styles.actions}>
           <IconButton icon="pencil" size={20} onPress={() => {
                            setSelectedSupplier(item);
                            setModalVisible(true);
                        }} />
           <IconButton icon="trash-can" size={20} iconColor="#B00020" onPress={() => handleDelete(item.id)} />
          </View>
        </View>
        {expandedId===item.id && (
          <View style={styles.details}>
            <Divider style={styles.divider}/>
            <List.Item
            title="Vergi numarası"
            description={item.taxNumber || "Belirtilmemiş"}
            left={props => <List.Icon{...props} icon="file-document-outline"/>}
            />
            <List.Item
            //List.Item bir satır olusturur
            title="Telefon"
            description={item.phone || "Belirtilmemiş"}
            left={ props => <List.Icon{...props} icon ="phone"/>}
            />
            <List.Item
            title="E-Posta"
            description={item.email || "Belirtilmemiş"}
            left={props => <List.Icon{...props} icon="email"/>}
            />
            <List.Item
            title="Adres"
            description={item.address || "Belirtilmemiş"}
            descriptionNumberOfLines={3} // adress max 3 satır gorunsun
            left={props=><List.Icon{...props} icon="map-marker"/>}
            />
          </View>
        )}
      </Card.Content>
    </Card>
  );
  return (
    <View style={styles.container}>
      {loading ? (
        <ActivityIndicator animating={true} style={{flex:1}}/>
      ):(
        <FlatList
        data={suppliers}//array aldı
        keyExtractor={(item)=> item.id.toString()} //her item için benzersiz key
        renderItem={renderItem}//her ıtem nasıl gorunecek yukarda yazdıgım gıbı 
        contentContainerStyle={styles.listContent}/>
      )}
      <FAB
      icon="plus"
      style={styles.fab}
      onPress={()=>{
        setSelectedSupplier(null);
        setModalVisible(true);
      }}
      />
      <SupplierModal
      visible={modalVisible}
      onDismiss={()=>setModalVisible(false)}
      onRefresh={listSuppliers}
      supplier={selectedSupplier}
      />
    </View>
  );
};
const styles=StyleSheet.create({
  container:{
    flex:1,
    background:"#f8f9fa"
  },
  listContent:{
    padding:12,
    paddingBottom:100
  },
  card:{
    marginBottom:12,
    backgroundColor:"#c7dbe6",
    borderRadius:8,
    elevation:3 //gölge derinlik olusturur
  },
  cardHeader:{
    flexDirection:"row",
    justifyContent:"space-between",
    alignItems:"center"
  },
  titleGroup:{
    flexDirection:"row",
    alignItems:"center"
  },
  suppliersName:{
    fontWeight:"bold",
    color:"#1a1a1a"
  },
  actions:{
    flexDirection:"row"
  },
  details:{
    marginTop:8,
    backgroundColor:"#e2e2e2",
    borderRadius:4
  },
  divider:{
    marginVertical:4
  },
  fab:{
    position:"absolute",
    margin:16,
    right:0,
    bottom:40,
    backgroundColor:"#397c91"
  }
});
export default SupplierScreen;