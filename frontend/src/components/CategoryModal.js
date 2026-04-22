import React, { useState, useEffect } from 'react';
import { StyleSheet, View } from 'react-native';
import { Modal, Portal, TextInput, Button, Text, IconButton } from 'react-native-paper';
import api from '../services/api';

const CategoryModal = ({ visible, onDismiss, onRefresh, category }) => {
    const [name, setName] = useState('');
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (category) {
            setName(category.name);
        } else {
            setName('');
        }
    }, [category, visible]);

    const handleSave = async () => {
        if (!name.trim()) {
            //trim bastakı ve sondakı boslugu sıler
            //ve bosluk olur js de bosluk false olur 
            //unlemle bunu true edıp parantezın ıcıne gırer
            alert("Lütfen kategori adı girin.");
            return;
        }

        setLoading(true);
        try {
            if (category) {
                // guncelleme modalda gelen categorı ıd  varsa 
                await api.put(`api/Category/${category.id}`, { id: category.id, name: name });
                alert("Kategori güncellendi.");
            } else {
                // ekleme cotogarı ıd yoksa 
                await api.post(`api/Category`, { name: name });
                alert("Yeni kategori eklendi.");
            }
            onRefresh();
            onDismiss();
        } catch (err) {
            if (err.response?.status === 400) alert("Bu isimde bir kategori zaten var!");
            else alert("Bir hata oluştu.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Portal>
            <Modal visible={visible} onDismiss={onDismiss} contentContainerStyle={styles.container}>
                <View style={styles.header}>
                    <Text style={styles.title}>{category ? "Kategoriyi Düzenle" : "Yeni Kategori"}</Text>
                    <IconButton icon="close" onPress={onDismiss} />
                </View>

                <TextInput
                    label="Kategori Adı"
                    value={name}
                    onChangeText={setName}
                    mode="outlined"
                    style={styles.input}
                    autoFocus
                />

                <Button 
                    mode="contained" 
                    onPress={handleSave} 
                    loading={loading}
                    style={styles.button}
                >
                    {category ? "Güncelle" : "Kaydet"}
                </Button>
            </Modal>
        </Portal>
    );
};

const styles = StyleSheet.create({
    container: { backgroundColor: 'white', padding: 20, margin: 20, borderRadius: 12 },
    header: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 15 },
    title: { fontSize: 18, fontWeight: 'bold' },
    input: { marginBottom: 20 },
    button: { paddingVertical: 5 ,backgroundColor:"#B5A0B8" }
});

export default CategoryModal;