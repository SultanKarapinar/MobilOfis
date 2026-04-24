import React , {useEffect,useState}from 'react';
import {FlatList, View, StyleSheet,Alert} from 'react-native';
import {Card, Text,FAB,IconButton,Divider,ActivityIndicator,List} from 'react-native-paper';
import api from '../services/api';
import UserModal from '../components/UserModal';



const UserScreen = () => {
 const [users,setUsers]=useState([]);
 const [loading,setLoading]=useState(false);
 const[expandedId,setExpandedId]=useState(null);
 const [modalVisible,setModalVisible]=useState(false);
 const [selectedUser,SetSelectedUser]=useState(null);

 useEffect (()=>{
  listUsers();
 },[]);

 const listUsers= async()=>{
  setLoading(true);
  try {
    const res=await api.get(`api/Users`);
    setUsers(res.data);
  } catch (error) {
    console.error(error);
    Alert.alert ("Hata","Kullanıcılar yüklennemedi.");
  }finally{
    setLoading(false);
  }
 };
 
 const handleDelete=(id)=>{
  Alert.alert("Emin misiniz? " , "Kullanıcı kaydı tamamen silinecektir.",[
    {text:"İptal",style:"cancel"},
    {text:"Sil",style:"destructive", onPress:async()=>{
      try{
        await api.delete(`api/Users/${id}`);
        setUsers(prev=>prev.filter(s=>s.id !==id));
      }catch(error){
        Alert.alert("Hata","Erişim izniniz yok veya bir hata oldu.");
      }
    }}
  ]);
 };
 const renderItem=({item})=>(
  <Card
  style={styles.card}
  onPress={()=>setExpandedId(expandedId===item.id ? null:item.id)}>
    <Card.Content>
      <View style={styles.cardHeader}>
        <View style={styles.titleGroup}>
          <IconButton icon="account" size={24} iconColor="#fff"/>
          <View>
            <Text variant="titleMedium" style={styles.usersName}>{item.name}</Text>
            <Text variant="bodySmall" > ID:{item.id}</Text>
          </View>
      </View>
          <View style={styles.actions}>
            <IconButton icon="pencil" size={20} onPress={()=>{
              SetSelectedUser(item);
              setModalVisible(true);
            }}/>
            <IconButton icon="trash-can"size={20} iconColor="#b00020" onPress={()=> handleDelete(item.id)}/>
          </View>
        </View>
        {expandedId===item.id &&(
          <View style={styles.details}>
            <Divider style= {styles.divider}/>
            <List.Item
            title="Ad Soyad"
            description={item.name ||"Belirtilmemiş" }
            left={ props=> <List.Icon{...props} icon="file-document-outline"/> } 
            />
            <List.Item
            title="E-Mail"
            description={item.email || "Belirtilmemiş"}
            left={props => <List.Icon{...props} icon="file-document-outline"/>}
            />
             <List.Item
            title="Rol"
            description={item.role || "Belirtilmemiş"}
            left={props => <List.Icon{...props} icon="file-document-outline"/>}
            />
             <List.Item
            title="Şifre"
            description={"********" &&
              "Şifre güvenlik gereği bu ekrandan değiştirilemez."
            }
            left={props => <List.Icon{...props} icon="file-document-outline"/>}
            />
          </View>
        )}
   
    </Card.Content>
  </Card>

 );
 return(
  <View style={styles.container}>
    {loading ? (
      <ActivityIndicator animating={true} style={{flex:1}}/>
    ):(
      <FlatList
      data={users}
      keyExtractor={(item)=>item.id.toString()}
      renderItem={renderItem}
      contentContainerStyle={styles.listContent}/>
    )}
    <FAB
    icon="plus"
    style={styles.fab}
    onPress={()=>{
      SetSelectedUser(null);
      setModalVisible(true);
    }}/>
    <UserModal
    visible={modalVisible}
    onDismiss={()=>setModalVisible(false)}
    onRefresh={listUsers}
    user={selectedUser}
    />
  </View>
 )

};
const styles=StyleSheet.create({
  container:{
     flex:1,
     backgroundColor:"#f8f9fa"
  },
  listContent:{
    padding:12,
    paddingBottom:100
  },
  card:{
    marginBottom:12,
    backgroundColor:"#ffcaaf ",
    borderRadius:8,
    elevation:3
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
    backgroundColor:"#ffcaaf ",
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
    backgroundColor:"#ffcaaf "
  }
});


export default UserScreen;