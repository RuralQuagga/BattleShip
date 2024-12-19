import { CellType } from "@/models/field/fieldMatrix";
import { Field } from "../Field/Field";
import { ThemeProps, useThemeColor, Text } from "../Themed";
import { View as DefaultView, StyleSheet } from "react-native"
import { useEffect, useState } from "react";

export type BattlefieldProps = ThemeProps & DefaultView['props'];

export function Battlefield(props: BattlefieldProps){
    const { lightColor, darkColor, ...otherProps } = props;
    const [myField, setMyField] = useState<CellType[][]>([[]]);
    const [apponentField, setApponentField] = useState<CellType[][]>([[]]);
    
    useEffect(() => {
        //call endpoint which return data for my field and apponent field
        let data: CellType[][] = [[]];
        for(let i = 0; i < 10; i++)
        {        
            let line: CellType[] = [];
            for(let j = 0; j < 10; j++)
            {
                line[j] = CellType.Ship;                
            }
            data[i] = line;
        }

        setMyField(data);   
        setApponentField(data);     
    },[]);        

  return(<>
    <Field {...otherProps} Items={myField}/>
    <Field {...otherProps} Items={apponentField}/>
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