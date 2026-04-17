import React, { useState, useEffect } from 'react';
import { StyleSheet, View, ScrollView,Alert} from 'react-native';
import { Modal, Portal, TextInput, Button, Text, IconButton } from 'react-native-paper';
import { Dropdown } from 'react-native-element-dropdown';
import api from '../services/api';

const StockActionModal = ({ visible, onDismiss, onRefresh, selectedProduct, actionType }) => {
    // Web projesindeki state'lerin birebir aynısı
    const [quantity, setQuantity] = useState(null);
    const [description, setDescription] = useState("");
    const [supplierId, setSupplierId] = useState(null);
    const [purchasePrice, setPurchasePrice] = useState(null);
    const [suppliers, setSuppliers] = useState([]);
    const [loading, setLoading] = useState(false);

    // Birim eşleştirmesi (Web'deki unitOfMeasureMap)
    const unitOfMeasureMap = { 0: "Kg", 1: "Paket", 2: "Litre", 3: "Adet" };

    useEffect(() => {
        // Stok artırma (actionType === 1) ise tedarikçileri yükle
        if (visible && actionType === 1) {
            api.get("/api/Suppliers").then(res => setSuppliers(res.data));
        }
        // Modal her açıldığında form temizlensin (openStockModal mantığı)
        if (visible) {
            setQuantity(null);
            setDescription("");
            setSupplierId(null);
            setPurchasePrice(null);
        }
    }, [visible, actionType]);

    const submitStock = async () => {
        // Web'deki validasyonun aynısı
       
        if (!quantity || parseFloat(quantity) <= 0) {
            Alert.alert("Miktar 0'dan büyük olmalı");
            return;
        }

        const datas = {
            productId: selectedProduct.id,
            quantity: parseFloat(quantity),
            transactionType: actionType === 1 ? 1 : -1,
            transactionDate: new Date().toISOString(),
            description: description
        };

        // Web'deki artırma işlemi ek alan mantığı
        if (actionType === 1) {
            datas.supplierId = supplierId ? Number(supplierId) : null;
            datas.unitPrice = purchasePrice ? Number(purchasePrice) : null;
        }

        setLoading(true);
        try {
            await api.post("/api/StockTransactions", datas);
           Alert .alert("Stok işlemi başarılı");
            onRefresh(); // list() fonksiyonunu tetikler
            onDismiss(); // Modal'ı kapatır
        } catch (error) {
             console.log("Giden Veri Kontrolü:", datas);
            console.error("Stok hatası:", error.response?.data);
           Alert. alert("İşlem sırasında bir hata oluştu.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Portal>
            <Modal visible={visible} onDismiss={onDismiss} contentContainerStyle={styles.container}>
                <View style={styles.header}>
                    <Text style={styles.title}>
                        {actionType === 1 ? "Stok Artır" : "Stok Azalt"}
                    </Text>
                    <IconButton icon="close" onPress={onDismiss} />
                </View>

                <ScrollView>
                    <View style={styles.subtitleBox}>
                        <Text style={styles.productName}>{selectedProduct?.name}</Text>
                        <Text style={styles.unitText}>
                            Birim: {unitOfMeasureMap[selectedProduct?.unitOfMeasure] ?? "-"}
                        </Text>
                    </View>

                    <TextInput
                        label="Miktar"
                        value={quantity?.toString()}
                        onChangeText={setQuantity}
                        mode="outlined"
                        keyboardType="decimal-pad" // Sayısal klavye
                        style={styles.input}
                    />

                    <TextInput
                        label="Açıklama"
                        value={description}
                        onChangeText={setDescription}
                        mode="outlined"
                        multiline
                        numberOfLines={3}
                        style={styles.input}
                        placeholder="Açıklama giriniz"
                    />

                    {/* Web'deki {actionType === 1 && ...} bloğu */}
                    {actionType === 1 && (
                        <>
                            <Text style={styles.label}>Tedarikçi</Text>
                            <Dropdown
                                style={styles.dropdown}
                                data={suppliers}
                                labelField="name"
                                valueField="id"
                                placeholder="Tedarikçi Seçin"
                                value={supplierId}
                                onChange={item => setSupplierId(item.id)}
                            />

                            <TextInput
                                label="Birim Fiyat"
                                value={purchasePrice?.toString()}
                                onChangeText={setPurchasePrice}
                                mode="outlined"
                                keyboardType="decimal-pad"
                                style={styles.input}
                            />
                        </>
                    )}

                    <Button 
                        mode="contained" 
                        onPress={submitStock} 
                        loading={loading}
                        style={styles.saveButton}
                        buttonColor={actionType === 1 ? "#4CAF50" : "#FF9800"}
                    >
                        Kaydet
                    </Button>
                </ScrollView>
            </Modal>
        </Portal>
    );
};

const styles = StyleSheet.create({
    container: {
        backgroundColor: 'white',
        padding: 20,
        margin: 20,
        borderRadius: 12,
        maxHeight: '85%'
    },
    header: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        marginBottom: 10
    },
    title: {
        fontSize: 20,
        fontWeight: 'bold'
    },
    subtitleBox: {
        backgroundColor: '#f5f5f5',
        padding: 10,
        borderRadius: 8,
        marginBottom: 15
    },
    productName: {
        fontSize: 16,
        fontWeight: '600'
    },
    unitText: {
        fontSize: 12,
        color: '#666'
    },
    input: {
        marginBottom: 12
    },
    label: {
        fontSize: 14,
        marginBottom: 5,
        color: '#666'
    },
    dropdown: {
        height: 50,
        borderColor: '#ccc',
        borderWidth: 1,
        borderRadius: 5,
        paddingHorizontal: 10,
        marginBottom: 12
    },
    saveButton: {
        marginTop: 10,
        paddingVertical: 5
    }
});

export default StockActionModal;