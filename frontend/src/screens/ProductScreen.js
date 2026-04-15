import React,{useState,useEffect} from 'react';
import { StyleSheet ,View,ScrollView , Alert ,ActivityIndicator} from 'react-native';
import{List,Card ,Badge,Button,FAB, Text,Provider} from 'react-native_paper';
import api from './api/axios';
import { ImageBackground } from 'react-native/types_generated/index';

const ProductScreen=()=>{
    const [products,setProducts]=useState([]);
    const [loading ,setLoading] = useState(true);
    const [filter,setFilter] = useState('all');

    useEffect(()=>{
        listProducts();
    },[]);

    const  listProducts=async()=>{
        try{
          //  setLoading(true);
            const res=await api.get("/api/products");
            setProducts(res.data);
        }
        catch(error)
        {
            Alert.alert("Hata, Ürünler yüklenirken hata olustu.")
        }
    }
    const listLowStock=async()=>{
        try{
            const res=await api.get("/api/Products/lowstock");
            setProducts(res.data);
        }catch(error)
        {
            Alert.alert("Hata:Azalan stoklar getirilemedi")
        }
    };
    const deleteProduct=(id)=>{
        Alert.alert("Silme Onayı","Bu ürünü silmek istediğinize emin misiniz?",[
            {text:"İptal",style:"cancel"},
            {text:"Sil",
                style:"destructive",
                onPress:async()=>{
                    try{
                        await api.delete(`/api/Products/${id}`);
                        setProducts(products.filter(p => p.id !== id));
                        Alert.alert("Başarılı","Ürün silindi.");
                    }
                    catch(eror)
                    {
                        Alert.alert("Hata","Erişim izniniz yok veya bir hata oldu");

                    }
                }
            }
        ]);

    };
    return(
        <Provider>
            <View style={styles.container}>
                <View style={styles.topContainer}>
                    <Button
                    mode={filter==='all' ? 'contained':'outlined' }
                    onPress={()=>{setFilter('all');listProducts();}}
                    style={styles.filterBtn}
                    labelStyle={{color:'#fff'}}>Tüm Ürünler</Button>

                    <Button
                    mode={filter==='low' ? 'cotained' : 'outlined'}
                    onPress={listLowStock}
                    style={styles.filterBtn}
                    labelStyle={{color:'#fff'}}>Stoku Azalanlar</Button>
                </View>
          
            { loading ? (
                <ActivityIndicator size="large" color="#92af71" style={{flex:1}}/>
            ) :(
                <ScrollView style={styles.scrollContainer}>
                    {products.map((item)=>(
                        <Card key ={item.id} style={styles.card}>
                            <List.Accordion
                            title={item.name}
                            left={props =><List.Icon{...props} icon="package-variant-closed"/>}
                            right={()=>(
                                <Badge
                                            style={{ backgroundColor: item.currentStock <= item.reorderLevel ? '#d32f2f' : '#2e7d32' }}
                                            size={26}> {item.currentStock}</Badge>
                                    )}>
                                        <View style={styles.details}>
                                            <View style={styles.row}>
                                                <Text style={styles.label}>Kategori</Text>
                                                <Text style={syyles.value}>{item.categori ||'Belirtilmemiş'} </Text>

                                            </View>
                                            <View style={styles.row}>
                                            <Text style={styles.label}>Güncel Stok Seviyesi</Text>
                                            <Text style={styles.value}>{item.currentStock}</Text>
                                        </View>
                                        <View style={styles.row}>
                                            <Text style={styles.label}>Min Stok Seviyesi</Text>
                                            <Text style={styles.value}>{item.reorderLevel}</Text>
                                        </View>
                                        <View style={styles.actionButtons}>
                                            <Button icon="pencil" mode="contained" onPress={() => {}} style={styles.button}>Düzenle</Button>
                                            <Button icon="trash-can" mode="contained" onPress={() => deleteProduct(item.id)} style={styles.button}>Sil</Button>
                                            <Button icon="plus" mode="contained" onPress={()=>{}} style={styles.button}>Stok </Button>
                                            <Button icon="minus" mode="contained" onPress={() => {}} style={styles.button}>Stok </Button>
                                        </View>

                                        </View>
                                    </List.Accordion>
                        </Card>
                    ))}
                </ScrollView>
            )}
            <FAB
                    icon="plus"
                    style={styles.fab}
                    onPress={() => {  }}
                />
              </View>
        </Provider>
    );
} ;
const styles = StyleSheet.create({
    container: { flex: 1, backgroundColor: '#f5f5f5' },
    topContainer: {
        flexDirection: 'row',
        padding: 10,
        justifyContent: 'space-around',
        backgroundColor: '#fff',
        elevation: 4,
    },
    filterBtn: {
        flex: 1,
        marginHorizontal: 5,
        backgroundColor: '#98ad80',
    },
    scrollContainer: { padding: 10 },
    card: { marginBottom: 10, elevation: 2 },
    details: { padding: 12, gap: 10, backgroundColor: '#f9f9f9' },
    row: { flexDirection: 'row', justifyContent: 'space-between' },
    label: { color: '#666', fontSize: 14 },
    value: { fontWeight: 'bold', fontSize: 15 },
    actionButtons: {
        flexDirection: 'row',
        flexWrap: 'wrap',
        justifyContent: 'space-between',
        marginTop: 10
    },
    button: {
        width: '48%',
        marginBottom: 8,
    },
    fab: {
        position: 'absolute',
        margin: 20,
        right: 10, 
        bottom: 20,
        backgroundColor: '#92af71'
    },
});

export default ProductScreen;