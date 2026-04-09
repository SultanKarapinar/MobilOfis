import { Provider as PaperProvider } from 'react-native-paper';
import React from 'react';

import { TouchableOpacity, Text } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { createDrawerNavigator } from '@react-navigation/drawer';

import LoginScreen from './src/screens/LoginScreen';
import HomeScreen from './src/screens/HomeScreen';
import CategoryScreen from './src/screens/CategoryScreen';
import SupplierScreen from './src/screens/SupplierScreen';
import ProductScreen from './src/screens/ProductScreen';
import UserScreen from './src/screens/UserScreen';

const stack = createStackNavigator();
const Drawer= createDrawerNavigator();

 function MyDrawer(){
  return(
    <Drawer.Navigator screenOptions={{drawerStyle: {
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
  options={({ navigation }) => ({ // navigation objesini buradan alıyoruz
    title: 'Ana Sayfa',
    headerLeft: () => (
      <TouchableOpacity 
        onPress={() => navigation.openDrawer()} // Menüyü açar
        style={{ marginLeft: 15 }}
      >
       
        <Text style={{ color: 'white', fontSize: 24, fontWeight: 'bold',marginRight:5, }}>☰</Text>
      </TouchableOpacity>
    ),
  })} 
/>

      <Drawer.Screen name="Ürünler" component={ProductScreen}
      options={({navigation})=>
      ({title:'Ürünler',
        headerLeft:()=>(
          <TouchableOpacity onPress={()=>navigation.openDrawer()}
          style={{marginLeft:15}}>
           <Text style={{ color: 'white', fontSize: 24, fontWeight: 'bold',marginRight:5, }}>☰</Text>  
          </TouchableOpacity>
        ),
      })}/>
      <Drawer.Screen name="Kategoriler" component={CategoryScreen}
       options={({ navigation }) => ({ 
    title: 'Kategoriler',
    headerLeft: () => (
      <TouchableOpacity 
        onPress={() => navigation.openDrawer()} // Menüyü açar
        style={{ marginLeft: 15 }}>
        
        <Text style={{ color: 'white', fontSize: 24, fontWeight: 'bold',marginRight:5, }}>☰</Text>
      </TouchableOpacity>
    ),
  })} >
        
      </Drawer.Screen>
      <Drawer.Screen name="Kullanıcılar" component={UserScreen}
       options={({ navigation }) => ({ 
    title: 'Kullanıcılar',
    headerLeft: () => (
      <TouchableOpacity 
        onPress={() => navigation.openDrawer()} // Menüyü açar
        style={{ marginLeft: 15 }}>
        <Text style={{ color: 'white', fontSize: 24, fontWeight: 'bold',marginRight:5, }}>☰</Text>
      </TouchableOpacity>
    ),
  })} >
        
      </Drawer.Screen>
      

      
      <Drawer.Screen name="Tedarikçiler" component={SupplierScreen}></Drawer.Screen>
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

