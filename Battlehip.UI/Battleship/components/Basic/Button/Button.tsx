import {
  GestureResponderEvent,
  Pressable,
  StyleSheet,
  Text,
  Image,
  TextStyle,
  ViewStyle,
  ImageSourcePropType,
  ImageStyle,
} from 'react-native';

type ButtonProps = {
  onPress: (event: GestureResponderEvent) => void;
  title: string;
  imgSource?: ImageSourcePropType;
  btnStyle?: ViewStyle;
  pressedBtnStyle?: ViewStyle;
  textStyle?: TextStyle;
  imgStyle?: ImageStyle; 
  disabled?: boolean;
};

export const Button = (props: ButtonProps) => {
  return (
    <Pressable    
      disabled={props.disabled}
      onPress={props.onPress}
      style={({ pressed }) => [
        [style.button, props.btnStyle],
        pressed && [style.buttonPressed, props.pressedBtnStyle],
      ]}
    >
      {props.imgSource ? (
        <Image style={[style.image, props.imgStyle]} source={props.imgSource} />
      ) : (
        <Text style={[style.text, props.textStyle]}>{props.title}</Text>
      )}
    </Pressable>
  );
};

const style = StyleSheet.create({
  button: {
    backgroundColor: '#6200ee',
    padding: 15,
    borderRadius: 10,
    alignItems: 'center',
    margin: 10,
  },
  buttonPressed: {
    opacity: 0.7,
  },
  text: {
    color: 'white',
    fontSize: 16,
  },
  image:{
    width: 50,
    height: 50,
  }
});
