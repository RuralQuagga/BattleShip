import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';
import { ImageBackground, StyleSheet, Text, View } from 'react-native';

type Props = NativeStackScreenProps<RootStackParamList, 'StartPage'>;

const StartPage = ({ navigation }: Props) => {
  return (
    <>
    <ImageBackground 
        source={require('../../assets/startPageBackground.svg')}
        style={style.imageBackground}
        resizeMode='cover'>
    <View style={style.container}>
        <Text style={style.gameName}>BattleShip</Text>
      </View>
    </ImageBackground>      
    </>
  );
};

const style = StyleSheet.create({
    container:{
        flex: 1,                                
    },
    imageBackground:{
        flex: 1,
        width: '100%',
        height: '100%'
    },
    gameName:{
        flex: 1,
        alignSelf:'center',
        marginTop: '30%',
        fontSize: 40,
        fontWeight: 600,
        fontFamily: 'sans-serif-medium',
        color: '#F0FFFF',        
    }
})

export default StartPage;
