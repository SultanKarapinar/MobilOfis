import React from 'react';
import { View, Text,StyleSheet} from 'react-native';

const StockTransactionScreen =()=>
{
    return(
        <View style={style.center}>
            <Text style={style.text}> Stok işlemleri sayfası</Text>
        </View>
    );
};
const style=StyleSheet.create({
     center:{flex: 1,  justifyContent:'center',alignItems:'center' },
    text:{fontSize:18,color:'#666'}
}
   
);
export default StockTransactionScreen;