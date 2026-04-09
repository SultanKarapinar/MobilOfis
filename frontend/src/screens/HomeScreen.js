import React from 'react';
import { Text, View, StyleSheet, TouchableOpacity,ScrollView} from 'react-native';
import MenuCard from '../components/MenuCard';
import CategoryScreen from './CategoryScreen';
const HomeScreen = ({ navigation }) => { 

    const exit = () => {
        navigation.navigate('Login');
    };

    return (
<ScrollView style={styles.container}>

    <View style={styles.row}>
               
        <MenuCard 
          icon="📦" 
          title="Ürünler" 
          onPress={() => navigation.navigate('Ürünler')} 
        />
        <MenuCard 
          icon="📁" 
          title="Kategoriler" 
          onPress={() => navigation.navigate('Kategoriler')} 
        />
     </View>
           
         
    <View style={styles.row}>
        <MenuCard 
          icon="📧" 
          title="E-Posta Ayarları" 
          onPress={() => navigation.navigate('E-Posta Ayarları')} 
        />
        <MenuCard 
          icon="➡️📧" 
          title=" Gönderilen E-Postalar " 
          onPress={() => navigation.navigate('Gönderilen E-Postalar')} 
        />
    </View>
           
    <View style={styles.row}>
        <MenuCard 
          icon="📊" 
          title="Stok İşlemleri" 
          onPress={() => navigation.navigate('Stok İşlemleri')} 
        />
        <MenuCard 
          icon="🤝" 
          title="Tedarikçiler" 
          onPress={() => navigation.navigate('Tedarikçiler')} 
        />
    </View>
            
    <View style={styles.row}>
        <MenuCard 
          icon="👥" 
          title="Kullanıcılar" 
          onPress={() => navigation.navigate('Kullanıcılar')} 
        />
    </View>
          
            <TouchableOpacity style={styles.button} onPress={exit}>
                <Text style={styles.buttonText}> Çıkış</Text>
            </TouchableOpacity> 
</ScrollView>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#f0f2f5', 
        padding: 20,
    },
    row:{
        flexDirection: 'row', //  yan yana dizer
        justifyContent: 'space-between', // kartların arasına eşit boşlu
        marginBottom: 15,
       

    },
    button: {
        marginTop: 'auto',
        backgroundColor: '#c0392b',
        padding: 15,
        borderRadius: 10,
        alignItems: 'center',
    },
    buttonText: { color: '#fff', fontWeight: 'bold' }
   
});

export default HomeScreen;