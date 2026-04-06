
import * as React from 'react';
import { View, Text, StyleSheet } from 'react-native';

// Tip tanımlamasını buraya taşıdık
interface ProductProps {
  name: string;
  stock: number;
}

const ProductCard: React.FC<ProductProps> = ({ name, stock }) => {
  return (
    <View style={styles.card}>
      <Text style={styles.name}>{name}</Text>
      <Text style={styles.stock}>Stok: {stock} Adet</Text>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: '#f1f1d7',
    padding: 15,
    borderRadius: 8,
    marginVertical: 8,
    borderLeftWidth: 5,
    borderLeftColor: '#d6a622',
    elevation: 2,
  },
  name: { fontSize: 18, fontWeight: 'bold' },
  stock: { fontSize: 14, color: '#666' },
});

export default ProductCard;