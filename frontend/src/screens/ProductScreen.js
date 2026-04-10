import React ,{useState} from 'react';
import {StyleSheet,ScrollView} from 'react-native';
import {DataTable,Card,Checkbox, Button, Searchbar,Surface} from 'react-native-paper';
import { View } from 'react-native-reanimated/lib/typescript/Animated';

const ProductScreen =()=>
{
    const [page,setPage]=useState(0);
    const itemsPerPage=5;

  const products=[ {id:1,name:'çay',category:'mutfak',CurrentStock:2 ,ReorderLevel:1,CreatedDate:111}, ];
    
    const from =page*itemsPerPage;//kacıncı urunde basladıhını bulmak
    const to=Math.min ((page+1)* itemsPerPage, products.length) //bitecegi indeks min ile tasmayı engelle
    return(
        <ScrollView style={styles.container}>
          
            <Card style={styles.card}>
                <DataTable>
                    <DataTable.Header>
                    <DataTable.Title>ID</DataTable.Title>
                    <DataTable.Title>Ürün </DataTable.Title>
                    <DataTable.Title>Kategori</DataTable.Title>
                    <DataTable.Title>Güncel Stok</DataTable.Title>
                    <DataTable.Title>Min Stok Seviyesi</DataTable.Title>
                    <DataTable.Title>Oluşturulma Tarihi</DataTable.Title>
                    <DataTable.Title>Güncelleme Tarihi</DataTable.Title>
                    </DataTable.Header>
                     {products.map((item)=>
                      <DataTable.Row key={item.id}>
                        <DataTable.Cell>{item.id}</DataTable.Cell>
                        <DataTable.Cell>{item.name}</DataTable.Cell>
                        <DataTable.Cell>{item.category}</DataTable.Cell>
                        <DataTable.Cell>{item.CurrentStock}</DataTable.Cell>
                        <DataTable.Cell>{item.ReorderLevel}</DataTable.Cell>
                        <DataTable.Cell>{item.CreatedDate}</DataTable.Cell>
                        <DataTable.Cell>{item.UpdatedDate}</DataTable.Cell>
                        
                        
                    </DataTable.Row>
                    )}
                    <DataTable.Pagination
                    page={page}
                    numberOfPages={Math.ceil(products.length/itemsPerPage)}//yukarı yuvarlam
                    onPageChange={(page)=>setPage(page)}
                    label={`${from + 1}-${to} / ${products.length}`}
                    numberOfItemsPerPage={itemsPerPage}
                    showFastPaginationControls
                    />
                </DataTable>

            </Card>


        </ScrollView>
    );
};
const styles=StyleSheet.create({
    container:{
        flex:1,
        padding:10,
        backgroundColor:'#f5f5f5',
    },
    toolbar: {
    padding: 15,
    backgroundColor: '#fff',
    borderBottomLeftRadius: 15,
    borderBottomRightRadius: 15,
    marginBottom: 10,
  },
  buttonRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 10,
  },
    card:{
        elevation:4,
        borderRadius:5,
        backgroundColor:'#fff',

    },
    header:{
        backgroundColor:'#e8f5e9'
    }

})
export default ProductScreen;