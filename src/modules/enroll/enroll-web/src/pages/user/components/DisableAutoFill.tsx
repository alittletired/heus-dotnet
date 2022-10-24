import React from 'react'

/*防止自动填充 */
export default function DisableAutoFill() {
  return (
    <React.Fragment>
      <input style={{opacity: 0, position: 'absolute'}} />
      <input type="password" style={{opacity: 0, position: 'absolute'}} />
    </React.Fragment>
  )
}
