import {
  GestureResponderEvent,
  Pressable,
  StyleSheet,
  Text,
  TextStyle,
  ViewStyle,
} from 'react-native';

type ButtonProps = {
  onPress: (event: GestureResponderEvent) => void;
  title: string;
  btnStyle?: ViewStyle;
  pressedBtnStyle?: ViewStyle;
  textStyle?: TextStyle;
};

export const Button = (props: ButtonProps) => {
  return (
    <Pressable
      onPress={props.onPress}
      style={({ pressed }) => [
        [style.button, props.btnStyle],
        pressed && [style.buttonPressed, props.pressedBtnStyle],
      ]}
    >
      <Text style={[style.text, props.textStyle]}>{props.title}</Text>
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
});
