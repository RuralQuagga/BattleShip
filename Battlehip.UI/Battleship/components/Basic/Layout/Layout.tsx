import {
  GestureResponderEvent,
  ImageBackground,
  StyleSheet,
  View,
} from 'react-native';
import { Button } from '../Button/Button';

type LayoutProps = {
  children: React.ReactNode;
  onBacckBtn?: (event: GestureResponderEvent) => void;
};

export const Layout = (props: LayoutProps) => {
  return (
    <ImageBackground
      source={require('../../../assets/startPageBackground.svg')}
      style={style.imageBackground}
      resizeMode='cover'
    >
      <View style={style.contentConteiner}>{props.children}</View>
      <View style={style.backButton}>
        {props.onBacckBtn && (
          <Button
            onPress={props.onBacckBtn}
            title='<'
            btnStyle={style.btnStyle}
            textStyle={style.btnName}
          />
        )}
      </View>
    </ImageBackground>
  );
};

const style = StyleSheet.create({
  imageBackground: {
    flex: 1,
    width: '100%',
    height: '100%',
  },
  backButton: {
    flex: 1,
    marginBottom: '10%',
  },
  contentConteiner: {
    flex: 9,
  },
  btnStyle: {
    width: '20%',
    backgroundColor: 'rgba(138,213,238,0.9)',
    borderBlockColor: '#002C4C',
    padding: 10,
  },
  btnName: {
    fontSize: 30,
    fontWeight: 600,
    fontFamily: 'sans-serif-medium',
    color: '#002C4C',
  },
});
