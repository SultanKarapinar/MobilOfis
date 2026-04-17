import { Provider as PaperProvider } from 'react-native-paper';
import React from 'react';

import { TouchableOpacity, Text,View } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { createDrawerNavigator,DrawerItemList ,DrawerItem, DrawerContentScrollView} from '@react-navigation/drawer';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';

import LoginScreen from './src/screens/LoginScreen';
import HomeScreen from './src/screens/HomeScreen';
import CategoryScreen from './src/screens/CategoryScreen';
import SupplierScreen from './src/screens/SupplierScreen';
import ProductScreen from './src/screens/ProductScreen';
import UserScreen from './src/screens/UserScreen';
import StockTransactionScreen from './src/screens/StockTransactionScreen';
import EmailNotificationSettingScreen from './src/screens/EmailNotificationSettingScreen';
import EmailNotificationSentScreen from './src/screens/EmailNotificationSentScreen';



const stack = createStackNavigator();
const Drawer= createDrawerNavigator();

//agac yapısını yapısını sayfalar ve alt kısmı ckıs butonu
    function CustomDrawerContent(props) {
  return (
    <View style={{ flex: 1, }}>
      
      <DrawerContentScrollView {...props} 
      >
        <DrawerItemList {...props} />
      </DrawerContentScrollView>

     
      <View
        style={{
          paddingBottom: 50,
          borderTopWidth: 1,
          borderColor: '#ccc',
       
        }}
      >
        <DrawerItem
          label="Çıkış Yap"
          labelStyle={{ color: 'red', fontWeight: 'bold' }}
          icon={({ color, size }) => (
            <Icon name="logout" color={color} size={size} />
          )}
          onPress={() => props.navigation.replace("Login")}
        />
      </View>
    </View>
  );
}
const Hamburger = ({ navigation }) => (
  <TouchableOpacity 
    onPress={() => navigation.openDrawer()}
    style={{ marginLeft: 15 }}
  >
    <Text style={{ 
      color: 'white', 
      fontSize: 24, 
      fontWeight: 'bold',
      marginRight: 5 
    }}>
      ☰
    </Text>
  </TouchableOpacity>
);

 function MyDrawer(){
  return(
    <Drawer.Navigator
    drawerContent={(props) => <CustomDrawerContent {...props} />}
     screenOptions={{drawerStyle: {
      backgroundColor: '#d6d6dd', 
      width: 250,
    },
    // Menüdeki yazıların 
    drawerLabelStyle: {
      fontWeight: 'bold',
      fontSize: 16,
    },
    // Seçili olanın stili
    drawerActiveTintColor: '#5b8168', 
    drawerActiveBackgroundColor: '#e8f5e9', // eçiliyken arka plan
    // Seçili olmayanların 
    drawerInactiveTintColor: '#788596',
    
   
    headerShown: true,
   headerTintColor: '#faf8f8', 
    headerStyle: {
      backgroundColor: '#5b8168',},
    
  }} 
  
>
  <Drawer.Screen 
  name="Ana Sayfa" 
  component={HomeScreen} 
  options={({ navigation }) => ({
    headerShown: true, 
    title: 'Ana Sayfa',
     headerLeft: () => <Hamburger navigation={navigation} />,
  })} 
/>

  <Drawer.Screen name="Ürünler" component={ProductScreen}
      options={({navigation})=>
      ({title:'Ürünler',
        drawerIcon: ({ color, size }) => (
      <Icon name="package-variant-closed" color={color} size={size} />
    ),
        headerLeft:()=><Hamburger navigation={navigation} />,
  })}
  />
  <Drawer.Screen name="Kategoriler" component={CategoryScreen}
       options={({ navigation }) => ({ 
    title: 'Kategoriler',  drawerIcon: ({ color, size }) => (
      <Icon name="format-list-bulleted" color={color} size={size} />
    ),
    headerLeft: () => <Hamburger navigation={navigation} />,
  })} />
    

          <Drawer.Screen name="E-Posta Ayarları" component={EmailNotificationSettingScreen}
       options={({ navigation }) => ({ 
    title: 'E-Posta Ayarları',
    drawerIcon: ({ color, size }) => (
      <Icon name="email-edit-outline" color={color} size={size} />
    ),
    headerLeft: () =><Hamburger navigation={navigation} />,
  })} />
       <Drawer.Screen name="Gönderilen E-Postalar" component={EmailNotificationSentScreen}
       options={({ navigation }) => ({ 
    title: 'Gönderilen E-Postalar',  drawerIcon: ({ color, size }) => (
      <Icon name="email-check-outline" color={color} size={size} />
    ),
    headerLeft: () =><Hamburger navigation={navigation} />,
  })} />  
   
   <Drawer.Screen name="Stok İşlemleri" component={StockTransactionScreen}
      options={({navigation})=>
      ({
        title:'Stok İşlemleri',  drawerIcon: ({ color, size }) => (
      <Icon name="swap-vertical" color={color} size={size} />
    ),
        headerLeft:()=><Hamburger navigation ={navigation}/>
      })}/> 

      <Drawer.Screen name="Tedarikçiler" component={SupplierScreen}
      options={({navigation})=>
      ({
        title:'Tedarikçiler',  drawerIcon: ({ color, size }) => (
      <Icon name="truck-delivery-outline" color={color} size={size} />
    ),
        headerLeft:()=><Hamburger navigation ={navigation}/>
      })}/> 
    
      <Drawer.Screen name="Kullanıcılar" component={UserScreen}
       options={({ navigation }) => ({ 
    title: 'Kullanıcılar',
    drawerIcon: ({ color, size }) => (
      <Icon name="account-group-outline" color={color} size={size} />
    ),
    headerLeft: () =><Hamburger navigation={navigation} />,
  })} />
  
       
        
      

     
    </Drawer.Navigator>
  )
 }

 export default function App()
{
  return(
    <NavigationContainer>
      {/* ilk açılan sayfa */}
      <stack.Navigator initialRouteName="Login"> 
      <stack.Screen
       name="Login"
       component={LoginScreen}
       options={{headerShown:false}}  
      />
      <stack.Screen
      name="Main"
      component={MyDrawer}
       options={{headerShown:false}}  
      />

      
      </stack.Navigator>
    </NavigationContainer>
  )
};

