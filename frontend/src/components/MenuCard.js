import React from'react';
import {Text,TouchableOpacity,StyleSheet} from'react-native';

const MenuCard =({icon,title,onPress})=>{
    return(
        <TouchableOpacity style= {styles.card} onPress={onPress}>
            <Text style={styles.iconText}>{icon} </Text>
            <Text style={styles.titleText}>{title} </Text>

        </TouchableOpacity>

    );

};
const styles = StyleSheet.create({
  card: {
    backgroundColor: '#cfe0bb',
    flex: 1,
    height: 130,
    margin: 8,
    borderRadius: 15,
    justifyContent: 'center',
    alignItems: 'center',
    elevation: 4, // Android gölge
    shadowColor: '#000', // iOS gölge
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
  },
  iconText: {
    fontSize: 35,
    marginBottom: 10,
  },
  cardTitle: {
    fontSize: 14,
    fontWeight: '600',
    color: '#333',
  },
});
export default MenuCard;