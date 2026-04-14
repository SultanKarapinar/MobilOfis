import React ,{useState} from 'react';
import { StyleSheet, View, ScrollView} from 'react-native';
import {List,Card ,Badge,Button ,FAB,useTheme,Text} from 'react-native-paper';

const ProductScreen = ()=>{


    const  [filter,setFilter]=useState('all');

    const [products , setProducts]=useState([
        {id:1,name:'Çay',category:'Mutfak', CurrentStock:2,ReorderLevel:5},
        {id:2,name:'Kahve',category:'Mutfak', CurrentStock:15,ReorderLevel:10},
        {id:3,name:'Şeker',category:'Mutfak',CurrentStock:20,ReorderLevel:12},
    ]);

    const filteredProducts=filter==='low' ? products.filter(p=> p.CurrentStock <= p.ReorderLevel) : products;
   return(
    <View style={styles.container}>
        <View style={styles.filterContainer}>
            <Button
            mode={filter==='all' ? 'contained' :'outlined'}
            onPress={()=>setFilter('all')}
            style={styles.filterBtn}
            > Tüm Ürünler</Button>
            <Button
            mode= {filter==='low'?'contained':'outlined'}
            onPress={()=>setFilter('low')}
            style={styles.filterBtn}
           
            >Stoku Azalanlar</Button>
        </View>
        <ScrollView style={styles.scrollContainer}>
            {filteredProducts.map((item)=>(
                <Card key={item.id} style={styles.card}>
                    <List.Accordion //acılır liste 
                    title={item.name} 
                    left={props => <List.Icon{...props} icon="package-variant-closed"/>}
                    right={()=>(
                        <Badge //bildirim sayısı gostermek daire içinde sayı
                        style={{backgroundColor: item.CurrentStock <= item.ReorderLevel ? '#d32f2f' : '#2e7d32'}}
                        size={24}> {item.CurrentStock}</Badge>
                    )}>
                        <View style={styles.details}>
                        <View style={styles.row}>
                        <Text style={styles.label}>Kategori</Text>
                        <Text style={styles.value}>{item.category}</Text>
                        </View>

                        <View style={styles.row}>
                         <Text style={styles.label}>Güncel Stok</Text>
                         <Text style={styles.value}>{item.CurrentStock}</Text>
                         </View>

                        <View style={styles.row}>
                        <Text style={styles.label}>Min Stok</Text>
                        <Text style={styles.value}>{item.ReorderLevel}</Text>
                        </View>
                            <View style ={styles.actionButtons}>
                                <Button style={styles.button} icon="pencil" mode="contained" onPress={()=>{}}>Düzenle</Button>
                                <Button style={styles.button} icon ="trash-can" mode="contained"  onPress={()=>{}}>Sil</Button>
                                <Button style={styles.button} icon="plus" mode="contained" onPress={()=>{}}>Stok Arttır</Button>
                                <Button style={styles.button} icon="minus" mode="contained" onPress={()=>{}}>Stok Azalt</Button>
                            </View>
                        </View>
                    </List.Accordion>
                </Card>
            ))}
        </ScrollView>
        <FAB //kaysan bıle sabıt en alt kose 
        icon="plus"
        style={styles.fab}
        OnPrenss={()=>{}}/>
    </View>
   );

};

const styles= StyleSheet.create({
    container:{
        flex:1,
        backgroundColor:'#f5f5f5'
    },
    filterContainer:{
        flexDirection:'row',
        padding:10,
        justifyContent:'space-around',
        backgroundColor:'#fff',
        elevation:4,
    },
    filterBtn:{
        flex:1,
        marginHorizontal:5,
        backgroundColor:'#98ad80',
        color:'#ffff',
    },
    scrollContainer:{
        padding:10,
    },
    card:{
        marginBottom:10,
    },
   details: {
  padding: 12,
  gap: 10,
},

row: {
  flexDirection: 'row',
  justifyContent: 'space-between',
  },

label: {
  color: '#666',
  fontSize: 14,
},

value: {
  fontWeight: 'bold',
  fontSize: 15,
},
    actionButtons:{
        flexDirection:'row',
        flexWrap: 'wrap',   
        justifyContent:'space-evenly',
        
    },
    button:{
      width:'48%',
      marginBottom: 8,
     backgroundColor:"#8b80b4",
    },
    fab:{
        pasition:'absolute',
        margin:20,
        width:60,
        left:300,
        bottom:100,
        backgroundColor:'#92af71'
    },

});
export default ProductScreen;