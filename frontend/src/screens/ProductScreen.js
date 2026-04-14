import React ,{useState} from 'react';
import { StyleSheet, View, ScrollView} from 'react-native';
import {List,Card ,Badge,Button ,FAB,useTheme,Text} from 'react-native-paper';

const ProductScreen = ()=>{


    const  [filter,setFilter]=useState('all');

    const [products , setProducts]=useState([
        {id:1,name:'Çay',category:'Mutfak', CurrentStock:2,ReorderLevel:5},
        {id:2,name:'Kahve',category:'Mutfak', CurrentStock:15,Reorder:10 },
        {id:3,name:'Şeker',category:'Mutfak',CurrentStock:20,ReorderLevel:5},
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
                    description={`Kategori:${item.category}` }
                    left={props => <List.Icon{...props} icon="package-variant-closed"/>}
                    right={()=>(
                        <Badge //bildirim sayısı gostermek daire içinde sayı
                        style={{backgroundColor: item.CurrentStock <= item.ReorderLevel ? '#d32f2f' : '#2e7d32'}}
                        size={24}> {item.CurrentStock}</Badge>
                    )}>
                        <View style={styles.details}>
                            <List.Item title="Ürün Adı" description={item.name} compact />
                            <List.Item title ="Min Stok" description={item.ReorderLevel} compact />
                            <View style ={styles.actionButtons}>
                                <Button icon="pencil" mode="text" onPress={()=>{}}>Düzenle</Button>
                                <Button icon ="trash-can" mode="text" color="red" onPrenss={()=>{}}>Sil</Button>
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
    },
    scrollContainer:{
        padding:10,
    },
    card:{
        marginBottom:10,
    },
    details:{
        padding:10,
        backgroundColor:'#fafafa',
    },
    actionButtons:{
        flexDirection:'row',
        justifyContent:'flex-end'
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