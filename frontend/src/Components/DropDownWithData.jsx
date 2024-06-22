

export const DropDownWithData = ({label, name, id, optionsArray, selectedOption}) => {


    return(
        <>
        <label htmlFor={name}>{label} :</label>

        <select name={name} id={id}>
            {optionsArray.map((option, index) => (
                <option value={option} key={index} name={name}>{option}</option>
            ))}

        </select>
        </>
    )


}