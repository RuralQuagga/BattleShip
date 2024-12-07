import { ThemeProps, useThemeColor, Text } from "../Themed";
import { View as DefaultView, StyleSheet } from "react-native"

export type BattlefieldProps = ThemeProps & DefaultView['props'];

export function Battlefield(props: BattlefieldProps){
    const { lightColor, darkColor, ...otherProps } = props;
  const backgroundColor = useThemeColor({ light: lightColor, dark: darkColor }, 'background');

  return(<>
  <DefaultView style={style.field} {...otherProps}>
    <Text style={style.fieldName}>My</Text>
  </DefaultView>
  <DefaultView style={style.field} {...otherProps}/>
  </>)
    
  
}

const style = StyleSheet.create({
    field: {
        backgroundColor: '#8aa7db',
        margin: '1%',
        height: '49%',
        width: '96%'
    },
    fieldName: {
        fontSize: 15,
        fontWeight: "600",
        opacity: 0.5,
        color: 'black'
    }
})