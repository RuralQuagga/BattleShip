import { StyleSheet, View, ViewStyle } from 'react-native';

type CenteredColumnProps = {
  children: React.ReactNode;
  style?: ViewStyle;
};

const CenteredColumn = (props: CenteredColumnProps) => {
  return <View style={[style.container, props.style]}>{props.children}</View>;
};

const style = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    padding: 16,
  },
});

export default CenteredColumn;
