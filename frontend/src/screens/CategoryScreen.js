import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const CategoryScreen = () => {
  return (
    <View style={styles.center}>
      <Text style={styles.text}>Kategoriler Sayfası</Text>
    </View>
  );
};

const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: 'center', alignItems: 'center', backgroundColor: '#f5f5f5' },
  text: { fontSize: 18, color: '#666' }
});

export default CategoryScreen;