import React from 'react';
import { SafeAreaView, ScrollView, StyleSheet, Text } from 'react-native';
import ProductCard from './src/components/ProductCard';

const App = () => {
  // Bu veriler ileride C# API'den (JSON olarak) gelecek
  const productList = [
    { id: 1, name: "Ofis Koltuğu", stock: 12 },
    { id: 2, name: "Kablosuz Mouse", stock: 45 },
    { id: 3, name: "Sultann", stock: 8 },
  ];

  return (
    <SafeAreaView style={styles.container}>
      <Text style={styles.header}>📦 Merkez Stok Takip</Text>
      <ScrollView contentContainerStyle={styles.list}>
        {productList.map((item) => (
          <ProductCard key={item.id} name={item.name} stock={item.stock} />
        ))}
      </ScrollView>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f5f5f5' },
  header: { fontSize: 24, fontWeight: 'bold', padding: 20, textAlign: 'center' },
  list: { paddingHorizontal: 20 },
});

export default App;