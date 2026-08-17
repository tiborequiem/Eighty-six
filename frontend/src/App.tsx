import { useState } from 'react'
import { GoogleLogin, type CredentialResponse } from '@react-oauth/google'
import axios from 'axios'

const BACKEND_URL = import.meta.env.VITE_BACKEND_URL

function App() {
  const [appToken, setAppToken] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const handleSuccess = async (credentialResponse: CredentialResponse) => {
    setError(null)
    setLoading(true)

    const idToken = credentialResponse.credential

    if (!idToken) {
      setError('No credential returned from Google.')
      setLoading(false)
      return
    }

    try {
      const response = await axios.post(`${BACKEND_URL}/api/auth/google`, {
        idToken: idToken,
      })

      setAppToken(response.data.token)
    } catch (err) {
      console.error(err)
      if (axios.isAxiosError(err)) {
        setError(
          err.response?.data?.toString() ??
            err.message ??
            'Login failed while contacting backend.'
        )
      } else {
        setError('Unexpected error during login.')
      }
    } finally {
      setLoading(false)
    }
  }

  const handleError = () => {
    setError('Google sign-in failed or was cancelled.')
  }

  return (
    <div style={{ maxWidth: 480, margin: '80px auto', textAlign: 'center', fontFamily: 'sans-serif' }}>
      <h1>Sign in</h1>

      <GoogleLogin onSuccess={handleSuccess} onError={handleError} />

      {loading && <p>Logging in…</p>}

      {error && (
        <p style={{ color: 'red', marginTop: 16 }}>{error}</p>
      )}

      {appToken && (
        <div style={{ marginTop: 24, textAlign: 'left' }}>
          <h3>App JWT</h3>
          <textarea
            readOnly
            value={appToken}
            rows={8}
            style={{ width: '100%', fontFamily: 'monospace', fontSize: 12 }}
          />
        </div>
      )}
    </div>
  )
}

export default App